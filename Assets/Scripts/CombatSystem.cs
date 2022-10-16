// System

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// Unity
using UnityEngine;

// Otherworld
using Otherworld.Core;

namespace Otherworld.Combat
{
    /// <summary>
    /// 
    /// </summary>
    public class CombatSystem : MonoBehaviour
    {
        [SerializeField] private CharacterList[] partyList;
        [SerializeField] private CombatBehaviour _combatBehaviour;

        private List<CombatBehaviour> _combatants;

        [SerializeField] private List<CombatBehaviour> currentTurn;
        [SerializeField] private string[] names;

        [SerializeField] private bool skipTurn = false;
        [SerializeField] private int turn = 0;
        [SerializeField] private int round = 0;


        [ContextMenu("Invoke OnSkipTurn()")]
        public void OnSkipTurn()
        {
            skipTurn = true;
        }

        [ContextMenu("Invoke OnCombatTrigger()")]
        public void OnCombatTrigger()
        {
            _combatants = new List<CombatBehaviour>();

            StartCoroutine(CombatCoroutine());
        }

        private IEnumerator CombatCoroutine()
        {
            var inCombat = true;

            // set the pawns to combat mode
            yield return SetCombatBehaviour();

            // run combat
            while (inCombat)
            {
                yield return ProcessRound();
            }

            // on combat ended
        }

        private IEnumerator ProcessRound()
        {
            round++;

            // randomize turn order
            var initiative = _combatants.OrderBy(x => Guid.NewGuid()).ToList();

            ShowDebugStringNames(initiative);

            // group party members with adjacent initiatives
            for (var i = 0; i < initiative.Count; i++)
            {
                if (i == 0)
                {
                    currentTurn.Add(initiative[i]);
                    continue;
                }

                if (initiative[i - 1].party == initiative[i].party)
                {
                    currentTurn.Add(initiative[i]);
                    continue;
                }

                yield return ProcessTurn();
                
                currentTurn.Add(initiative[i]);
            }

            yield return ProcessTurn();
        }

        private IEnumerator ProcessTurn()
        {
            foreach (var member in currentTurn)
            {
                member.StartTurn();
            }

            yield return new WaitUntil(() =>
            {
                foreach (var member in currentTurn)
                {
                    //if(member.IsTurnComplete()) continue;
                    if (skipTurn) continue;
                    return false;
                }

                return true;
            });

            // todo remove this???
            foreach (var member in currentTurn)
            {
                member.EndTurn();
            }
            
            currentTurn.Clear();
            skipTurn = false;
            turn++;
        }

        private void ShowDebugStringNames(List<CombatBehaviour> initiative)
        {
            // - // debug show names in inspector
            names = new string[initiative.Count];
            for (var i = 0; i < initiative.Count; i++)
            {
                names[i] = initiative[i].name;
            }
            // - // - - - - - - - - - - - - - - -
        }

        // TODO - rewrite to support more than 2 parties
        private IEnumerator SetCombatBehaviour()
        {
            var i = 0;
            foreach (var member in partyList[0])
            {
                var behaviour = Instantiate(_combatBehaviour);

                member.GetComponent<CharacterController>().SetInputBehaviour(behaviour);
                behaviour.Initialize(member.transform, partyList[0], partyList[1], this);

                behaviour.name = "Player " + i++;
                _combatants.Add(behaviour);
                yield return null;
            }

            foreach (var member in partyList[1])
            {
                var behaviour = Instantiate(_combatBehaviour);

                member.GetComponent<CharacterController>().SetInputBehaviour(behaviour);
                behaviour.Initialize(member.transform, partyList[1], partyList[0], this);

                behaviour.name = "Enemy " + i++;
                _combatants.Add(behaviour);
                yield return null;
            }
        }
    }
}
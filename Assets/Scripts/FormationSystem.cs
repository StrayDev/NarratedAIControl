using System;
using System.Collections.Generic;
using Otherworld.Core;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

[Serializable]
public class Row
{
    public int count = 3;
    public Vector3 Center = default;
    public Vector3 Direction = default;
    public (Vector3, Vector3) Line = default;
    public List<Vector3> Positions = default;
}

public class FormationSystem : MonoBehaviour
{
    // Data
    [SerializeField] private CharacterList party;
    [SerializeField] private InputProvider input;

    // Variables
    [SerializeField] private List<Row> rows = new List<Row>();

    [SerializeField] private float lerpSpeed = .01f;
    [SerializeField] private float _minRadius = .5f;

    private Vector3 _formationCenter;
    private float _formationRadius;
    private bool _breakFormation;

    private void OnEnable()
    {
        input.OnMoveEvent += InputOnOnMoveEvent;
    }

    private void InputOnOnMoveEvent(Vector2 v)
    {
        if (v == Vector2.zero) return;
        var direction = new Vector3(v.x, 0, v.y);
        rows[0].Direction = direction;
    }

    private void Update()
    {
        // update leader position
        rows[0].Center = new Vector3(party[0].position.x, 0, party[0].position.z);

        if (rows[0].Direction == Vector3.zero) return;

        if (!_breakFormation)
        {
            UpdateLineFormationPositions();
        }
        else
        {
            rows[0].Positions[0] = rows[0].Center;

            // create the lines and positions
            for (var r = 0; r < rows.Count; r++)
            {
                for (var p = 0; p < rows[r].Positions.Count; p++)
                {
                    OffsetOtherPositions(r, p);
                }
            }
        }
        UpdateFormationData();
        UpdateBrakeFormation();
    }

    private void OffsetOtherPositions(int row, int pos)
    {
        var position = rows[row].Positions[pos];

        for (var r = 0; r < rows.Count; r++)
        {
            // skip previous rows
            if (r < row) continue;

            for (var p = 0; p < rows[r].Positions.Count; p++)
            {
                // skip previous positions
                if (row == r && p < pos) continue;

                var otherPosition = rows[r].Positions[p];
                if (Vector3.Distance(position, otherPosition) < _minRadius * 2)
                {
                    // move other position away
                    var direction = (otherPosition - position).normalized;
                    var offset = direction * (_minRadius * 2);
                    rows[r].Positions[p] = position + offset;
                }
            }
        }
    }

    private void UpdateBrakeFormation()
    {
        var leader = rows[0].Positions[0] + (rows[0].Direction * _minRadius);

        // do nothing if in inner formation
        var inRange = Vector3.Distance(leader, _formationCenter) > _formationRadius - _minRadius * 2;


        // use the dot for if moving in to the formation
        var direction = rows[0].Direction;

        var vector = (leader - _formationCenter).normalized;

        var dot = Vector3.Dot(vector, direction);

        Debug.Log(dot);

        _breakFormation = dot < .25f || !inRange;
    }


    private void UpdateFormationData()
    {
        var start = Vector3.zero;
        var end = Vector3.zero;
        var distance = 0.0f;

        foreach (var r1 in rows)
        {
            foreach (var p1 in r1.Positions)
            {
                foreach (var r2 in rows)
                {
                    foreach (var p2 in r2.Positions)
                    {
                        var dist = Vector3.Distance(p1, p2);
                        if (dist > distance)
                        {
                            distance = dist;
                            start = p1;
                            end = p2;
                        }
                    }
                }
            }
        }

        _formationCenter = (start + end) / 2;
        _formationRadius = distance / 2 + _minRadius;
    }

    private void UpdateLineFormationPositions()
    {
        // set the center positions of each row to follow the last
        for (var i = 1; i < rows.Count; i++)
        {
            rows[i].Center = SetFollowPosition(rows[i-1].Center, rows[i].Center);
        }

        // set the direction for each row
        for (var i = 1; i < rows.Count; i++)
        {
            rows[i].Direction = SetDirection(rows[i].Center, rows[i-1].Center);
        }

        // create the lines and positions
        foreach (var row in rows)
        {
            // set the line for each row
            row.Line = SetLine(row);

            // set positions along each line
            SetPositions(row);
        }

        // resolve collisions
        for (var i = 1; i < rows.Count; i++)
        {
            for (var x = i - 1; x > -1; x--)
            {
                ResolveCollisions(rows[i], rows[x]);
            }
        }

    }

    private void ResolveCollisions(Row current, Row priority)
    {
        for (var i = 0; i < current.count; i++)
        {
            for (var j = 0; j < priority.count; j++)
            {
                var c = current.Positions[i];
                var p = priority.Positions[j];

                var dist = Vector3.Distance(c, p);
                if(dist > _minRadius * 2.001) continue;

                var vector = c - p;
                var target = p + vector.normalized * (_minRadius * 2);
                current.Positions[i] = Vector3.Lerp(c, target, lerpSpeed);
            }
        }
    }

    private void SetPositions(Row row)
    {
        if (row.count != row.Positions.Count)
        {
            row.Positions = new List<Vector3>();
            for (var c = 0; c < row.count; c++)
            {
                row.Positions.Add(Vector3.zero);
            }
        }

        var diameter = _minRadius * 2;

        for (var i = 0; i < row.count; i++)
        {
/*            var d = (row.Line.Item2 - row.Line.Item1).normalized;
            var target = row.Line.Item1 + ((diameter * d) * i) + (d * _minRadius);
            row.Positions[i] = Vector3.Lerp(row.Positions[i], target, lerpSpeed / 2);*/


            var odd = row.count % 2 == 0 ? 1 : 0;
            var side = i % 2 == 0 ? 1 : -1;

            var lineDirection = (row.Line.Item2 - row.Line.Item1).normalized;
            
            var centerOffset = (lineDirection * _minRadius) * odd;
            
            var positionOffset = (lineDirection * (_minRadius * 2)) * (Mathf.CeilToInt(i / 2.0f)) ;

            row.Positions[i] = row.Center + centerOffset + (positionOffset * side);

        }
    }

    private Vector3 SetDirection(Vector3 self, Vector3 target)
    {
        return (target - self).normalized;
    }


    private (Vector3, Vector3) SetLine(Row row)
    {
        // do nothing if 0,0,0
        if (row.Direction == Vector3.zero) return row.Line;

        var ra = new Vector3(-row.Direction.z, 0, row.Direction.x);
        ra.Normalize();

        var l = row.Center + ra * (_minRadius * row.count);
        var r = row.Center - ra * (_minRadius * row.count);
        
        return (r, l);
    }

    private Vector3 SetFollowPosition(Vector3 target, Vector3 self)
    {
        var vector = self - target;
        var t = target + vector.normalized * (_minRadius * 2);
        return Vector3.Lerp(self, t, lerpSpeed);
    }

   


    private void OnDrawGizmos()
    {
        // draw formation boundary 
        Handles.color = _breakFormation ? Color.green : Color.magenta;
        Handles.DrawWireDisc(_formationCenter, Vector3.up, _formationRadius);
        Handles.color = _breakFormation ? Color.magenta : Color.green;
        Handles.DrawWireDisc(_formationCenter, Vector3.up, _formationRadius - _minRadius);

        foreach (var r in rows)
        {
/*            Handles.color = Color.green;
            Handles.DrawWireDisc(r.Center, Vector3.up, _minRadius);*/

            Handles.color = Color.blue;
            Handles.DrawLine(r.Center, r.Center + r.Direction);

            Handles.color = Color.yellow;
            Handles.DrawLine(r.Line.Item1, r.Line.Item2);

            Handles.color = Color.green;
            foreach (var p in r.Positions)
            {
                Handles.DrawWireDisc(p, Vector3.up, _minRadius);
            }
        }
    }


    /*    private Vector3 _mid = Vector3.zero;

        private Vector3 leader = Vector3.zero;
        private Vector3 midFollow = Vector3.forward;
        private Vector3 backFollow = Vector3.forward * 2;

        private Vector3 lstart = Vector3.zero;
        private Vector3 lend = Vector3.zero;

        private List<(Vector3, Vector3)> _lines;

        private int _rows = 3;
        private List<int> _rowCounts;
        private List<Vector3> _centerList;*/


    /*    if (positions.Count< 1) return;

        Handles.color = Color.yellow;

        Handles.DrawWireDisc(positions[0], Vector3.up, _minRadius);
        Handles.DrawWireDisc(midFollow, Vector3.up, _minRadius);
        Handles.DrawWireDisc(backFollow, Vector3.up, _minRadius);


        Handles.color = Color.green;
        foreach (var (l, r) in _lines)
        {
            Handles.DrawLine(l, r);
        }*/

    /*        Handles.DrawLine(leader, midFollow);
    Handles.DrawLine(lstart, lend);

    Handles.DrawWireDisc(lstart, Vector3.up, _minRadius);
    Handles.DrawWireDisc(lend, Vector3.up, _minRadius);*/



    /*        foreach (var v in positions)
            {
                var position = new Vector3(v.x, 0, v.z);

                Handles.color = Color.green;
                Handles.DrawWireDisc(position, Vector3.up, _minRadius);

                Handles.color = Color.blue;

                var vector = positions[0] - position;

                var r1 = positions[0] - vector.normalized * _minRadius;
                var r2 = position + vector.normalized * _minRadius;

                Handles.DrawLine(r1, r2);
            }

            foreach (var v in positions2)
            {
                var position = new Vector3(v.x, 0, v.z);

                Handles.color = Color.green;
                Handles.DrawWireDisc(position, Vector3.up, _minRadius);

                Handles.color = Color.blue;

                var vector = positions[0] - position;

                var r1 = positions[0] - vector.normalized * _minRadius;
                var r2 = position + vector.normalized * _minRadius;

                Handles.DrawLine(r1, r2);
            }

            Handles.DrawWireDisc(_mid, Vector3.up, _minRadius);*/

/*    private void UpdateFormationPositions()
    {
        // track the leader position
        var vector = Vector3.zero;
        var leader = new Vector3(_leader.position.x, 0, _leader.position.z);
        positions[0] = leader;

        // skip 1 for the leader
        for (var i = 1; i < positions.Count; i++)
        {
            // offset position from leader
            vector = positions[i] - leader;
            var target = leader + vector.normalized * (_minRadius * 2);
            positions[i] = Vector3.Lerp(positions[i], target, lerpSpeed);

            var distance = Vector3.Distance(leader, positions[i]);

            // if leader is moving toward you
            if (distance < _minRadius * 2 + .01f)
            {
                continue;
            }*/

            /*            // stay adjacent to leader and neighbor
                        for (var j = 1; j < positions.Count; j++)
                        {
                            if (j == i) continue;
                            // get the mid point
                            _mid = new Vector3((positions[i].x + positions[j].x) / 2, 0, (positions[i].z + positions[j].z) / 2);
                            // get vector between the two points 
                            vector = positions[i] - positions[j];

                            // get the midpoint 
                            target = _mid + vector.normalized * _minRadius;
                            positions[i] = Vector3.Lerp(positions[i], target, lerpSpeed * 1.5f);
                        }*/
        

        // line 2 - - - - - - - - - - - - - - - - - - - - -
        /*        for (var i = 0; i < positions2.Count; i++)
                {
                    var distance = Vector3.Distance(leader, positions2[i]);

                    // if leader is moving toward you
                    if (distance < _minRadius * 4 + .01f)
                    {
                        foreach (var pos in positions)
                        {
                            // if close
                            var distance2 = Vector3.Distance(positions2[i], pos);

                            // if leader is moving toward you
                            if (distance2 < _minRadius * 2 + .01f)
                            {
                                // offset position from pos
                                vector = positions2[i] - pos;
                                var target2 = pos + vector.normalized * (_minRadius * 2);
                                positions2[i] = Vector3.Lerp(positions2[i], target2, lerpSpeed);
                            }
                        }
                        continue;
                    }

                    // offset position from leader
                    vector = positions2[i] - leader;
                    var target23 = leader + vector.normalized * (_minRadius * 4);
                    positions2[i] = Vector3.Lerp(positions2[i], target23, lerpSpeed);

                    //--
                    // stay adjacent to leader and neighbor
                    for (var j = 0; j < positions2.Count; j++)
                    {
                        if (j == i) continue;
                        // get the mid point
                        _mid = new Vector3((positions2[i].x + positions2[j].x) / 2, 0, (positions2[i].z + positions2[j].z) / 2);
                        // get vector between the two points 
                        vector = positions2[i] - positions2[j];

                        // get the midpoint 
                        var target5 = _mid + vector.normalized * _minRadius;
                        positions2[i] = Vector3.Lerp(positions2[i], target5, lerpSpeed * 1.5f);
                    }
                }*/
    

}

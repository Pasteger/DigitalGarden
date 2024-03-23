using System;
using System.Collections.Generic;
using UnityEngine;

public class TreeGenerator : MonoBehaviour
{
    public GameObject sphere;
    public int iteration;
    public float lenght;
    public float angle;
    public float thick;

    private float _thickLeaves;

    private readonly System.Random _random = new();

    private string _axiom = "22220";
    private readonly Dictionary<char, string> _rules = new();

    private readonly Stack<float> _stack = new();

    private Vector3 _drawPositionVector;
    private Vector3 _drawAngleVector;

    private void Start()
    {
        var position = transform.position;
        _drawPositionVector.x = position.x;
        _drawPositionVector.y = position.y;

        _drawAngleVector.x = 90;
        _drawAngleVector.z = 90;

        _thickLeaves = thick / 1.5f;

        _rules.Add('1', "21");
        _rules.Add('0', "1[-20]+20");

        for (var i = 0; i < iteration; i++)
        {
            var temp = "";
            foreach (var c in _axiom)
            {
                if (_rules.TryGetValue(c, out var rule))
                {
                    temp += rule;
                }
                else
                {
                    temp += c;
                }
            }

            _axiom = temp;
        }

        foreach (var c in _axiom)
        {
            switch (c)
            {
                case '+':
                    _drawAngleVector.x += angle - _random.Next(-13, 13);
                    _drawAngleVector.z += _random.Next(-90, 90);
                    break;
                case '-':
                    _drawAngleVector.x -= angle - _random.Next(-13, 13);
                    _drawAngleVector.z -= _random.Next(-90, 90);
                    break;
                case '2':
                    if (_random.Next(0, 10) > 4)
                        Draw(Color.black);
                    break;
                case '1':
                    if (_random.Next(0, 10) > 4)
                        Draw(Color.black);
                    break;
                case '0':
                    _stack.Push(thick);

                    thick = _thickLeaves;
                    var color = _random.Next(0, 10);
                    var brush = Color.green;
                    if (color < 3)
                        brush = Color.green;
                    else if (color < 6)
                        brush = Color.green;
                    else if (color < 10)
                        brush = Color.green;
                    Draw(brush);

                    thick = _stack.Pop();

                    break;
                case '[':
                    thick *= 0.85f;
                    _stack.Push(thick);
                    _stack.Push(_drawPositionVector.x);
                    _stack.Push(_drawPositionVector.y);
                    _stack.Push(_drawPositionVector.z);
                    _stack.Push(_drawAngleVector.x);
                    _stack.Push(_drawAngleVector.z);
                    break;
                case ']':
                    _drawAngleVector.z = _stack.Pop();
                    _drawAngleVector.x = _stack.Pop();
                    _drawPositionVector.z = _stack.Pop();
                    _drawPositionVector.y = _stack.Pop();
                    _drawPositionVector.x = _stack.Pop();
                    thick = _stack.Pop();
                    break;
            }
        }

        _drawPositionVector.x = position.x;
        _drawPositionVector.y = position.y;
        _drawPositionVector.z = position.z;
        _drawAngleVector.x = 90;
        _drawAngleVector.z = 90;
    }

    private void Draw(Color color)
    {
        var radAngleX = _drawAngleVector.x * (Mathf.PI / 180f);
        var radAngleZ = _drawAngleVector.z * (Mathf.PI / 180f);

        var xFinale = _drawPositionVector.x + lenght * (float)Math.Cos(radAngleX) * (float)Math.Cos(radAngleZ);
        var yFinale = _drawPositionVector.y + lenght * (float)Math.Sin(radAngleX);
        var zFinale = _drawPositionVector.z + lenght * (float)Math.Cos(radAngleX) * (float)Math.Sin(radAngleZ);

        var step = 0.3f * thick;
        var offset = 0;

        for (float i = 0; i < 1f; i += step)
        {
            if (offset > 1 / step) return;

            var xThis = _drawPositionVector.x + (xFinale - _drawPositionVector.x) * offset / (1f / step);
            var yThis = _drawPositionVector.y + (yFinale - _drawPositionVector.y) * offset / (1f / step);
            var zThis = _drawPositionVector.z + (zFinale - _drawPositionVector.z) * offset / (1f / step);

            var pos = new Vector3(xThis, yThis, zThis);

            var point = Instantiate(sphere, pos, Quaternion.identity);
            point.transform.localScale = new Vector3(thick, thick, thick);
            point.GetComponent<Renderer>().material.color = color;

            offset++;
        }

        _drawPositionVector.x = xFinale;
        _drawPositionVector.y = yFinale;
        _drawPositionVector.z = zFinale;
    }
}
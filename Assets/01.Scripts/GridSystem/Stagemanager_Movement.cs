using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public partial class StageManager
{
    public class GridMovement : IDisposable
    {

        private Vector2 _moveDir;
        private Vector2 _mousePositon;
        private Vector2 _originPoint;
        private Vector2 _targetPoint;
        private Cell _startCell;
        private bool _startMovement;
        private bool _endMovement;
        private CellInstance[] _instances;
        private CellInstance[] _copyObject;
        private GridData _data;
        private float _cellSize;
        private GridInputSO _input;

        public Transform pivot;
        public Action<LineType> moveEndCallback;
        public Action moveCancelCallback;
        public Vector2 startMousePos;

        public GridMovement(Cell startCell, Transform pivot,
            GridData data, float cellSize, Action<LineType> moveEndCallback,
            Action moveCancelCallback, GridInputSO input)
        {

            this.pivot = pivot;
            this.moveEndCallback = moveEndCallback;
            _startCell = startCell;
            _data = data;
            _cellSize = cellSize;
            _input = input;
            InputManager.ChangeInputMap(InputMapType.Grid);
            _input.OnGridEndEvent += HandleGridEnd;
            pivot.position = _originPoint = _mousePositon = startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            this.moveCancelCallback = moveCancelCallback;
        }

        private void HandleGridEnd()
        {

            if (_endMovement) return;

            if (_moveDir == Vector2.zero || Vector3.Distance(pivot.transform.position, _originPoint) < 0.5f)
            {

                pivot.Clear();
                moveCancelCallback?.Invoke();
                return;

            }

            _endMovement = true;
            _targetPoint = GetTargetPos(pivot.position);
            pivot.transform.position = _originPoint;

            TurnManager.Instance.SetTurnData(TurnDataType.IsPreview, false);
            TurnManager.Instance.EndCurrentTurn();

            pivot.Clear();
            _copyObject = new CellInstance[0];

            foreach (var item in _instances)
            {

                item.transform.SetParent(pivot);

            }

            Instance.StartCoroutine(MovementCo());

        }

        private Vector3 GetTargetPos(Vector3 target)
        {

            return _moveDir == Vector2.up ?
                new Vector2(target.x, Mathf.RoundToInt(target.y)) :
                new Vector2(Mathf.RoundToInt(target.x), target.y);

        }

        public void Update()
        {

            _mousePositon = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CheckStartMove();
            MoveCell();
            CheckOutBounce();

        }

        private void CheckOutBounce()
        {

            if (!_startMovement) return;

            foreach (var item in _instances)
            {
                
                if (CheckBounce(item.transform.position))
                {

                    var pos = item.transform.position;
                    item.transform.position = GetLineType() == LineType.Row
                        ? new Vector3((_cellSize * (_data.width * 3)) * GetSign(pos) / 2, pos.y)
                        : new Vector3(pos.x, (_cellSize * (_data.height * 3)) * GetSign(pos) / 2);

                }

            }

            foreach (var item in _copyObject)
            {

                if (CheckBounce(item.transform.position))
                {

                    var pos = item.transform.position;
                    item.transform.position = GetLineType() == LineType.Row
                        ? new Vector3((_cellSize * (_data.width * 3)) * GetSign(pos) / 2, pos.y)
                        : new Vector3(pos.x, (_cellSize * (_data.height * 3)) * GetSign(pos) / 2);


                }

            }

            if (CheckBounce(pivot.transform.position))
            {

                var pos = pivot.transform.position;
                pivot.transform.position = GetLineType() == LineType.Row
                    ? new Vector3((_cellSize * (_data.width * 3)) * (GetSign(pos) * -1) / 2, pos.y)
                    : new Vector3(pos.x, (_cellSize * (_data.height * 3)) * (GetSign(pos) * -1) / 2);

            }

        }

        private bool CheckBounce(Vector3 pos)
        {

            return GetLineType() == LineType.Row
                ? Mathf.Abs(pos.x) > (_cellSize * (_data.width * 3)) / 2
                : Mathf.Abs(pos.y) > (_cellSize * (_data.height * 3)) / 2;

        }

        private float GetSign(Vector3 pos)
        {

            return GetLineType() == LineType.Row
                ? pos.x > 0 ? -1 : 1
                : pos.y > 0 ? -1 : 1;

        }

        private void MoveCell()
        {

            if (_startMovement && !_endMovement)
            {

                pivot.position = GetPivotPositoin();

            }

        }

        private Vector3 GetPivotPositoin()
        {

            return _moveDir.y == 0 ? new Vector2(_mousePositon.x, pivot.position.y) : new Vector2(pivot.position.x, _mousePositon.y);

        }

        private void CheckStartMove()
        {

            if (_startMovement) return;

            if (Vector2.Distance(startMousePos, _mousePositon) > 0.5f)
            {

                _startMovement = true;
                _moveDir = GetMovementDir(_mousePositon - startMousePos);

                var source = Instance.Grid.GetCellInstnaces(_startCell, GetLineType());
                var copySourceP = Instance.Grid.CopyAndCollocateCellInstances(_startCell, GetLineType(), _data.width);
                var copySourceM = Instance.Grid.CopyAndCollocateCellInstances(_startCell, GetLineType(), -_data.width);
                _instances = new CellInstance[source.Length * 3];
                _copyObject = new CellInstance[source.Length * 3];
                Array.Copy(source, 0, _instances, 0, source.Length);
                Array.Copy(copySourceP, 0, _instances, source.Length, copySourceP.Length);
                Array.Copy(copySourceM, 0, _instances, source.Length * 2, copySourceM.Length);

                for (int i = 0; i < _instances.Length; i++)
                {

                    _copyObject[i] = Instantiate(_instances[i]);
                    _copyObject[i].CellData = _instances[i].CellData;

                    foreach (var item in _copyObject[i].GetComponentsInChildren<SpriteRenderer>())
                    {

                        item.sortingOrder = 100;
                        var old = item.color;
                        old.a = 0.4f;
                        item.color = old;

                    }

                    _copyObject[i].transform.SetParent(pivot);

                }

            }

        }

        private void EndMovement()
        {

            foreach (var item in _instances)
            {

                item.transform.SetParent(null);

            }

            moveEndCallback?.Invoke(GetLineType());

        }

        private LineType GetLineType()
        {

            return _moveDir == Vector2.right ? LineType.Row : LineType.Column;

        }

        private Vector2 GetMovementDir(Vector2 dir)
        {

            return Mathf.Abs(dir.x) > Mathf.Abs(dir.y) ? Vector2.right : Vector2.up;

        }

        private IEnumerator MovementCo()
        {

            float per = 0;

            while (per < 1)
            {

                per += Time.deltaTime * 2;
                pivot.transform.position = Vector2.Lerp(_originPoint, _targetPoint, per);
                yield return null;

            }

            TurnManager.Instance.SetTurnData(TurnDataType.IsMovementCell, false);
            EndMovement();

        }

        public void Dispose()
        {

            _input.OnGridEndEvent -= HandleGridEnd;

        }

    }

}

using Godot;
using System;

public partial class SwipePanel : Control
{
    [Export] private HBoxContainer _pagesContainer;
    [Export] private float _swipeThreshold = 100.0f; // 滑动判定阈值
    [Export] private float _swipeSpeed = 0.3f;       // 滑动动画速度
    [Export] private Label OsTxt;
    private Vector2 _dragStartPosition;
    private float _pageWidth;
    [Export]private int _currentPageIndex = 0;
    private Tween _currentTween;
    private bool _isDragging = false;
    public override void _Ready()
    {
        _pageWidth = GetViewportRect().Size.X;
        OsTxt.Text = OS.GetName();
        if (OS.GetName() == "Android")
        {
            _swipeThreshold = 200;
        }
    }


    public override void _Input(InputEvent @event)
    {
        // 统一处理触摸和鼠标输入
        if (@event is InputEventScreenTouch touchEvent)
        {
            HandleTouch(touchEvent);
        }
        else if (@event is InputEventMouseButton mouseButton && mouseButton.ButtonIndex == MouseButton.Left)
        {
            HandleMouse(mouseButton);
        }
        else if ((@event is InputEventScreenDrag dragEvent) ||
                (@event is InputEventMouseMotion mouseMotion && _isDragging))
        {
            HandleDrag(@event);
        }
    }

    private void HandleTouch(InputEventScreenTouch touchEvent)
    {
        if (touchEvent.Pressed)
        {
            StartDrag(touchEvent.Position);
        }
        else if (touchEvent.IsReleased())
        {
            EndDrag();
        }
    }

    private void HandleMouse(InputEventMouseButton mouseButton)
    {
        if (mouseButton.Pressed)
        {
            StartDrag(mouseButton.Position);
        }
        else
        {
            EndDrag();
        }
    }

    private void StartDrag(Vector2 position)
    {
        _dragStartPosition = position;
        _currentTween?.Kill(); // 停止当前动画
        _isDragging = true;
    }

    private void HandleDrag(InputEvent @event)
    {
        if (@event is InputEventScreenDrag dragEvent)
        {
            Vector2 delta = dragEvent.Position - _dragStartPosition;
            // 实时移动容器位置（限制在边界内）
            float targetX = Mathf.Clamp(
                _pagesContainer.Position.X + delta.X,
                -(_currentPageIndex + 1) * _pageWidth,
                -(_currentPageIndex - 1) * _pageWidth
            );
            _pagesContainer.Position = new Vector2(targetX, _pagesContainer.Position.Y);
            _dragStartPosition = dragEvent.Position;
        }
        else if (@event is InputEventMouseMotion mouseMotion)
        {
            Vector2 delta = mouseMotion.Position - _dragStartPosition;
            // 实时移动容器位置（限制在边界内）
            float targetX = Mathf.Clamp(
                _pagesContainer.Position.X + delta.X,
                -(_currentPageIndex + 1) * _pageWidth,
                -(_currentPageIndex - 1) * _pageWidth
            );
            _pagesContainer.Position = new Vector2(targetX, _pagesContainer.Position.Y);
            _dragStartPosition = mouseMotion.Position;
        }
    }

    private void EndDrag()
    {
        // 计算最终偏移方向
        float deltaX = _pagesContainer.Position.X + _currentPageIndex * _pageWidth;
        int direction = 0;

        if (Mathf.Abs(deltaX) > _swipeThreshold)
        {
            direction = deltaX > 0 ? -1 : 1;
        }

        SwitchPage(direction);
        _isDragging = false;
    }

    private void SwitchPage(int direction)
    {
        _currentPageIndex = Mathf.Clamp(_currentPageIndex + direction, 0, _pagesContainer.GetChildCount() - 1);
        AnimateToCurrentPage();
    }

    private void AnimateToCurrentPage()
    {
        float targetX = -_currentPageIndex * _pageWidth;
        _currentTween = CreateTween().SetEase(Tween.EaseType.Out);
        _currentTween.TweenProperty(_pagesContainer, "position:x", targetX, _swipeSpeed);
    }
}
using Godot;
using System;

public partial class FlowUi : Node
{
    [Export] private Control TextContainer; // 包含文本的容器
    [Export] private Button ToggleButton;   // 触发显示的按钮
    // 动画参数
    private Vector2 _hiddenPosition;
    private Vector2 _visiblePosition;
    private bool _isVisible = false;
    private Tween _currentTween;
    public override void _Ready()
    {
        // 确保所有必要节点都已设置
        if (TextContainer == null || ToggleButton == null)
        {
            GD.PrintErr("TextContainer or ToggleButton not assigned!");
            return;
        }

        // 初始化位置
        _visiblePosition = TextContainer.Position;
        _hiddenPosition = new Vector2(
            _visiblePosition.X - TextContainer.Size.X, // 向左移出屏幕
            _visiblePosition.Y
        );
        // 在_Ready中初始设置
        TextContainer.Modulate = new Color(1, 1, 1, 0);
        // 设置初始状态为隐藏
        TextContainer.Position = _hiddenPosition;
        TextContainer.Visible = true; // 确保容器可见（但位置在屏幕外）

        // 连接按钮信号
        ToggleButton.Pressed += OnToggleButtonPressed;
    }

    private void OnToggleButtonPressed()
    {
        ToggleTextVisibility();
    }

    public void ToggleTextVisibility()
    {
        // 清理现有动画
        _currentTween?.Kill();

        // 创建新动画
        _currentTween = CreateTween()
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Back);

        // 确定目标位置
        Vector2 targetPosition = _isVisible ? _hiddenPosition : _visiblePosition;

        // 执行滑动动画
        _currentTween.TweenProperty(TextContainer, "position", targetPosition, 0.7f);

        // 在ToggleTextVisibility中添加：
        _currentTween.Parallel().TweenProperty(TextContainer, "modulate:a", _isVisible ? 0f : 1f, 0.5f);
        // 更新状态
        _isVisible = !_isVisible;
    }

}

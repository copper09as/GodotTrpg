shader_type canvas_item;

// 流光参数
uniform vec4 glow_color : source_color = vec4(0.2, 0.6, 1.0, 0.7); // 流光颜色 (蓝)
uniform float speed = 1.5;                  // 流动速度
uniform float width = 0.25;                 // 流光宽度
uniform float intensity = 1.0;              // 效果强度
uniform float border_thickness = 2.0;       // 边框厚度

// 动画参数
uniform float active = 1.0;                 // 选中状态 (0-1)

void fragment() {
    vec4 base_color = texture(TEXTURE, UV);

    // 当控件被选中时显示效果
    if (active > 0.01) {
        // 计算流光位置 (随时间移动)
        float flow_pos = mod(UV.x * 2.0 + TIME * speed, 2.0);

        // 创建流光形状 (带状渐变)
        float glow = smoothstep(0.0, width, flow_pos) *
                    smoothstep(width * 2.0, width, flow_pos);

        // 应用强度曲线
        glow = pow(glow, 3.0) * intensity;

        // 添加边框效果
        float border = 0.0;
        if (UV.x < border_thickness / 100.0 || UV.x > 1.0 - border_thickness / 100.0 ||
            UV.y < border_thickness / 100.0 || UV.y > 1.0 - border_thickness / 100.0) {
            border = 0.3 * active;
        }

        // 混合颜色
        vec4 final_glow = glow_color * glow * active;
        COLOR = mix(base_color, base_color + final_glow, glow) + vec4(border);
    } else {
        COLOR = base_color;
    }
}
shader_type canvas_item;

// 雷电参数
uniform bool active = false;                 // 是否激活特效
uniform float intensity : hint_range(0, 5) = 1.0; // 特效强度
uniform float speed : hint_range(0, 5) = 1.0;    // 动画速度
uniform vec4 lightning_color = vec4(1.0, 0.8, 0.2, 1.0); // 金色 (RGB)

// 噪声纹理
uniform sampler2D noise_tex : source_color;

void fragment() {
    vec4 base_color = texture(TEXTURE, UV);

    if (active) {
        // 计算UV偏移 (随时间移动)
        vec2 uv_offset = vec2(TIME * speed * 0.2, TIME * speed * 0.3);

        // 获取噪声值
        float noise1 = texture(noise_tex, UV * 1.5 + uv_offset).r;
        float noise2 = texture(noise_tex, UV * 2.0 - uv_offset).g;

        // 计算边缘距离 (0=边缘, 1=中心)
        float edge_dist = min(
            min(UV.x, 1.0 - UV.x),
            min(UV.y, 1.0 - UV.y)
        );

        // 创建雷电效果
        float lightning = smoothstep(0.4, 0.6, noise1) *
                         smoothstep(0.3, 0.5, noise2) *
                         (1.0 - smoothstep(0.0, 0.1, edge_dist));

        // 增加脉动效果
        float pulse = 0.5 + 0.5 * sin(TIME * speed * 3.0);
        lightning *= pulse * intensity;

        // 创建发光效果
        vec3 glow = vec3(lightning) * lightning_color.rgb * 2.0;

        // 混合效果
        vec3 final_color = mix(base_color.rgb, lightning_color.rgb, lightning * 0.5);
        final_color += glow;

        // 保持原始透明度
        COLOR = vec4(final_color, base_color.a);
    } else {
        COLOR = base_color;
    }
}
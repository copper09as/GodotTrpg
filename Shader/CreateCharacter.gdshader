shader_type canvas_item;

uniform float time = 0.0;
uniform float speed = 1.0;
uniform float intensity = 1.0;

void fragment() {
    vec2 uv = FRAGCOORD.xy / SCREEN_PIXEL_SIZE;
    vec2 center = vec2(0.5, 0.5);
    float distance = length(uv - center);
    float glow = smoothstep(0.4, 0.5, distance) * intensity;
    glow *= sin(time * speed + distance * 10.0) * 0.5 + 0.5;
    COLOR = texture(TEXTURE, UV) * (1.0 + glow); // 修改原始纹理颜色
}
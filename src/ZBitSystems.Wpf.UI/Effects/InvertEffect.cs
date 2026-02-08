using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Effects;

namespace ZBitSystems.Wpf.UI.Effects;

/// <summary>
/// A pixel shader effect that inverts the colors of an element.
/// Useful for making dark-on-light images visible in dark mode themes.
/// </summary>
/// <example>
/// <code>
/// &lt;Image Source="logo.png"&gt;
///     &lt;Image.Effect&gt;
///         &lt;effects:InvertEffect /&gt;
///     &lt;/Image.Effect&gt;
/// &lt;/Image&gt;
/// </code>
/// </example>
public class InvertEffect : ShaderEffect
{
    private const string ShaderAsBase64 =
        @"AAP///7/HwBDVEFCHAAAAE8AAAAAA///AQAAABwAAAAAAQAASAAAADAAAAADAAAAAQACADgAAAAA
AAAAaW5wdXQAq6sEAAwAAQABAAEAAAAAAAAAcHNfM18wAE1pY3Jvc29mdCAoUikgSExTTCBTaGFk
ZXIgQ29tcGlsZXIgMTAuMQCrUQAABQAAD6AAAIA/AAAAAAAAAAAAAAAAHwAAAgUAAIAAAAOQHwAA
AgAAAJAACA+gQgAAAwAAD4AAAOSQAAjkoAIAAAMAAAeAAADkgQAAAKAFAAADAAgHgAAA/4AAAOSA
AQAAAgAICIAAAP+A//8AAA==";

    private static readonly PixelShader Shader;

    static InvertEffect()
    {
        Shader = new PixelShader();
        Shader.SetStreamSource(new MemoryStream(Convert.FromBase64String(ShaderAsBase64)));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvertEffect"/> class.
    /// </summary>
    public InvertEffect()
    {
        PixelShader = Shader;
        UpdateShaderValue(InputProperty);
    }

    /// <summary>
    /// Identifies the Input dependency property used as the shader sampler input.
    /// </summary>
    public static readonly DependencyProperty InputProperty =
        RegisterPixelShaderSamplerProperty("Input", typeof(InvertEffect), 0);
}

using System.Text;
using SharpGL.VertexBuffers;
using SharpGL;
using VE3NEA;
using SharpGL.Shaders;
using System.Buffers;
using Serilog;
using System.Windows.Forms;
using System.Diagnostics;

namespace OrbiNom
{
  // OpenGL Extensions Viewer: https://geeks3d.com/dl/show/10097


  public partial class WaterfallControl : UserControl
  {
    static bool OpenglInfoNeeded = true;

    private const int TEXTURE_WIDTH = 32768; // opengl limit
    private const int TEXTURE_FOLD = 4;      // one spectrum takes 4 rows in the texture
    private const int SPECTRA_HEIGHT = 2048; // full screen height for most screen sizes

    public const int SPECTRA_WIDTH = TEXTURE_WIDTH * TEXTURE_FOLD;
    private const int TEXTURE_HEIGHT = SPECTRA_HEIGHT * TEXTURE_FOLD;
    
    
    private VertexBufferArray VertexBufferArray;
    private ShaderProgram ShaderProgram;
    private IndexedTexture IndexedTexture;
    private Palette Palette = new();

    public float Fps;

    public float Brightness = 0;
    public float Contrast = 0.5f;
    public double Zoom = 1.95f;
    public double Pan = 0;




    //----------------------------------------------------------------------------------------------
    //                                        init
    //----------------------------------------------------------------------------------------------
    public WaterfallControl()
    {
      InitializeComponent();
    }

    private void OpenglControl_OpenGLInitialized(object sender, EventArgs e)
    {
      OpenGL gl = OpenglControl.OpenGL;

      CheckError(gl, false);

      LogOpenglInformation();

      gl.Disable(OpenGL.GL_DEPTH_TEST);
      CheckError(gl);

      ShaderProgram = new ShaderProgram();
      CheckError(gl);
      ShaderProgram.Create(gl,
        Encoding.ASCII.GetString(Properties.Resources.WaterfallVertexShader),
        Encoding.ASCII.GetString(Properties.Resources.WaterfallFragmentShader),
        null);
      CheckError(gl);
      ShaderProgram.AssertValid(gl);
      CheckError(gl);
      ShaderProgram.Bind(gl);
      CheckError(gl);

      gl.Uniform1(ShaderProgram.GetUniformLocation(gl, "indexedTexture"), 0);
      CheckError(gl);
      gl.Uniform1(ShaderProgram.GetUniformLocation(gl, "paletteTexture"), 1);
      CheckError(gl);

      IndexedTexture = new(OpenglControl.OpenGL, TEXTURE_WIDTH, TEXTURE_HEIGHT);
      SetPalette(Palette);

      CreateVba(gl);
    }

    private void LogOpenglInformation()
    {
      if (!OpenglInfoNeeded) return;
      OpenglInfoNeeded = false;

      OpenGL gl = OpenglControl.OpenGL;

      string str = gl.GetString(OpenGL.GL_VERSION);
      if (gl.GetError() != OpenGL.GL_NO_ERROR) str = "<Error>";
      Log.Information($"OpenGL version: {str}");

      str = gl.GetString(OpenGL.GL_VENDOR);
      if (gl.GetError() != OpenGL.GL_NO_ERROR) str = "<Error>";
      Log.Information($"OpenGL vendor: {str}");

      str = gl.GetString(OpenGL.GL_RENDERER);
      if (gl.GetError() != OpenGL.GL_NO_ERROR) str = "<Error>";
      Log.Information($"OpenGL renderer: {str}");

      str = gl.GetString(OpenGL.GL_SHADING_LANGUAGE_VERSION​);
      if (gl.GetError() != OpenGL.GL_NO_ERROR) str = "<Error>";
      Log.Information($"OpenGL shading language: {str}");
    }

    internal void SetPalette(Palette palette)
    {
      Palette = palette;
      IndexedTexture.SetPalette(Palette.Colors);
    }

    private void CreateVba(OpenGL gl)
    {
      CheckError(gl, false);

      VertexBufferArray = new VertexBufferArray();
      CheckError(gl);
      VertexBufferArray.Create(gl);
      CheckError(gl);
      VertexBufferArray.Bind(gl);
      CheckError(gl);

      var vertices = new float[] { -1, -1, -1, 1, 1, -1, 1, 1 };
      var vertexDataBuffer = new VertexBuffer();
      CheckError(gl);
      vertexDataBuffer.Create(gl);
      CheckError(gl);
      vertexDataBuffer.Bind(gl);
      CheckError(gl);
      vertexDataBuffer.SetData(gl, 0, vertices, false, 2);
      CheckError(gl);

      var texCoords = new float[] { 0, 1, 0, 0, 1, 1, 1, 0 };
      var texCoordsBuffer = new VertexBuffer();
      CheckError(gl);
      texCoordsBuffer.Create(gl);
      CheckError(gl);
      texCoordsBuffer.Bind(gl);
      CheckError(gl);
      texCoordsBuffer.SetData(gl, 1, texCoords, false, 2);
      CheckError(gl);

      VertexBufferArray.Unbind(gl);
      CheckError(gl);
    }

    internal static void CheckError(OpenGL gl, bool log = true)
    {
      uint err;

      while ((err = gl.GetError()) != OpenGL.GL_NO_ERROR)
        if (log)
        {
          string stackTrace = new System.Diagnostics.StackTrace(true).ToString();
          if (IsLoggableError(stackTrace))
          {
            string message = $"{gl.ErrorString(err)}\n{stackTrace}";
            Log.Error(message);
            Debug.WriteLine(message);
          }
        }
    }

    // AppendSpectrum may be called many times per second.
    // If an error occurs inside that method, do not log it every time
    private static DateTime NextLogErrortime = DateTime.MinValue;
    private static bool IsLoggableError(string stackTrace)
    {
      if (!stackTrace.Contains("AppendSpectrum")) return true;

      if (DateTime.UtcNow < NextLogErrortime) return false;
      NextLogErrortime = DateTime.UtcNow + TimeSpan.FromSeconds(30);
      return true;
    }




    //----------------------------------------------------------------------------------------------
    //                                      draw
    //----------------------------------------------------------------------------------------------
    private void OpenglControl_OpenGLDraw(object sender, SharpGL.RenderEventArgs args)
    {
      var gl = OpenglControl.OpenGL;

      CheckError(gl, false);

      ShaderProgram.Bind(gl);
      CheckError(gl);
      IndexedTexture.Bind(gl);
      CheckError(gl);
      VertexBufferArray.Bind(gl);
      CheckError(gl);

      //UpdateScrollPosOnDraw();
      float scrollHeight = OpenglControl.Size.Height / (float)SPECTRA_HEIGHT;

      // a bug in SharpGL prevents ShaderProgram.SetUniform1 from setting int 
      gl.Uniform1(ShaderProgram.GetUniformLocation(gl, "in_ScreenWidth"), OpenglControl.Size.Width);
      CheckError(gl);

      ShaderProgram.SetUniform1(gl, "in_zoom", (float)Zoom);
      CheckError(gl);
      ShaderProgram.SetUniform1(gl, "in_pan", (float)Pan);
      CheckError(gl);

      ShaderProgram.SetUniform1(gl, "in_ScrollPos", (float)ScrollPos);
      CheckError(gl);
      ShaderProgram.SetUniform1(gl, "in_ScrollHeight", scrollHeight);
      CheckError(gl);
      ShaderProgram.SetUniform1(gl, "in_Brightness", computeBrightness());
      CheckError(gl);
      ShaderProgram.SetUniform1(gl, "in_Contrast", Contrast);
      CheckError(gl);

      gl.DrawArrays(OpenGL.GL_TRIANGLE_STRIP, 0, 4);
      CheckError(gl);
      VertexBufferArray.Unbind(gl);
      CheckError(gl);

      ShaderProgram.Unbind(gl);
      CheckError(gl);
      UpdateFps();
    }

    // correct brightness for spectrum shrinking
    private float computeBrightness()
    {
      return Brightness + 0.03679f * (float)Math.Log(Zoom * ClientSize.Width) - 0.6657f;
    }

    int frameCount;
    DateTime startTime;
    private void UpdateFps()
    {
      DateTime currentTime = DateTime.Now;
      frameCount++;
      var elapsed = (currentTime - startTime).TotalSeconds;
      if (elapsed < 1) return;

      Fps = (float)(frameCount / elapsed);
      startTime = currentTime;
      frameCount = 0;
    }

    ArrayPool<float> ArrayPool = ArrayPool<float>.Shared;
    public int Row;                // write positon, 0..TEXTURE_HEIGHT-1
    private double ScrollPos = 0;  // display position in texture, 0..1

    internal void AppendSpectrum(float[] spectrum)
    {
      if (!IsHandleCreated || !Enabled) return;

      // spectrum may be overwritten after this function returns, make a copy
      var spectrumCopy = ArrayPool.Rent(spectrum.Length);
      Array.Copy(spectrum, spectrumCopy, spectrum.Length);

      BeginInvoke(() =>
      {
        IndexedTexture.SetRows(Row * TEXTURE_FOLD, TEXTURE_FOLD, spectrumCopy);
        ArrayPool.Return(spectrumCopy);

        if (++Row == SPECTRA_HEIGHT) Row = 0;
        ScrollPos = Row / (float)SPECTRA_HEIGHT;
        OpenglControl.Invalidate();
      });
    }
  }
}
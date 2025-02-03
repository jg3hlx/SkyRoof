using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpGL.VertexBuffers;
using SharpGL;
using WeifenLuo.WinFormsUI.Docking;
using SharpGL.Shaders;
using SGPdotNET.Observation;
using VE3NEA;
using System.Diagnostics;
using System.Drawing.Imaging;
using Serilog;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;

namespace OrbiNom
{
  public partial class EarthViewPanel : DockContent
  {
    private readonly Context ctx;
    private readonly Font RegularFont, BoldFont;
    private OpenGL gl;
    private ShaderProgram ShaderProgram;
    private VertexBufferArray VertexBufferArray;
    private uint[] TextureIds = new uint[1];
    private int VertexCount;
    private SpriteRenderer SpriteRenderer;
    private Sprite SatelliteSprite, HomeSprite;
    public GeoPoint Home, Center;
    private double Footprint;
    private double Azimuth;
    public double Zoom = 1;
    public SatnogsDbSatellite Satellite;


    public EarthViewPanel() { InitializeComponent(); }

    public EarthViewPanel(Context ctx)
    {
      InitializeComponent();
      this.ctx = ctx;
      ctx.EarthViewPanel = this;
      ctx.MainForm.EarthViewMNU.Checked = true;
      Home = GridSquare.ToGeoPoint(ctx.Settings.User.Square);
      SetSatellite();

      RegularFont = new Font(OrbitRadioBtn.Font, FontStyle.Regular);
      BoldFont = new Font(OrbitRadioBtn.Font, FontStyle.Bold);

      openglControl1.MouseWheel += OpenglControl1_MouseWheel;
    }

    private void EarthViewPanel_FormClosing(object sender, FormClosingEventArgs e)
    {
      ctx.EarthViewPanel = null;
      ctx.MainForm.EarthViewMNU.Checked = false;
    }

    private void openglControl1_Resize(object sender, EventArgs e)
    {
      openglControl1.Invalidate();
      ValidateZoom();

      OrbitRadioBtn.Text = $"Zoom {Zoom:F2}x Radius {Math.Min(openglControl1.ClientSize.Width, openglControl1.ClientSize.Height)}";
    }

    private void OpenglControl1_MouseWheel(object? sender, MouseEventArgs e)
    {
      Zoom *= 1 + e.Delta / 1200f;
      ValidateZoom();
      OrbitRadioBtn.Text = $"Zoom {Zoom:F2}x Radius {Math.Min(openglControl1.ClientSize.Width, openglControl1.ClientSize.Height)}";
    }

    private void ValidateZoom()
    {
      var diam = Math.Min(openglControl1.ClientSize.Width, openglControl1.ClientSize.Height);
      if (diam > 0) Zoom = Math.Max(1f, Math.Min(25000 / diam, Zoom));
    }

    internal void SetSatellite(SatnogsDbSatellite? satellite = null)
    {
      Satellite = satellite ?? ctx.SatelliteSelector.SelectedSatellite;
      SatelliteRadioBtn.Text = $"{Satellite.name}  Now";
      if (SatelliteRadioBtn.Checked) SatelliteRadioBtn.Font = new(SatelliteRadioBtn.Font, FontStyle.Bold);

      ComputeSatLocation();
      Zoom = 0.9 * Math.PI / Footprint;
      Invalidate();
    }

    public void SetPass(SatellitePass? pass)
    {

    }

    public void Advance()
    {
      if (!SatelliteRadioBtn.Checked || Satellite == null) return;

      ComputeSatLocation();
      Invalidate();
    }

    private void ComputeSatLocation()
    {
      var p = Satellite.Tracker.Predict().ToGeodetic();
      var nextP = Satellite.Tracker.Predict(DateTime.UtcNow.AddSeconds(10)).ToGeodetic();
      var nextCenter = new GeoPoint(nextP.Latitude.Degrees, nextP.Longitude.Degrees);

      Center = new(p.Latitude.Degrees, p.Longitude.Degrees);
      Footprint = p.GetFootprintAngle().Radians;
      Azimuth = (nextCenter - Center).AzimuthRad;
    }




    //----------------------------------------------------------------------------------------------
    //                                 init opengl objects
    //----------------------------------------------------------------------------------------------
    private void openglControl1_OpenGLInitialized(object sender, EventArgs e)
    {
      gl = openglControl1.OpenGL;
      CheckError(false);

      SpriteRenderer = new(gl);
      CheckError(false);
      SatelliteSprite = new Sprite(gl, Properties.Resources.satellite);
      HomeSprite = new Sprite(gl, Properties.Resources.ant);

      gl.Disable(OpenGL.GL_DEPTH_TEST);
      CheckError();

      LoadShaders();
      CreateVba();
      LoadTexture();

      gl.Uniform1(ShaderProgram.GetUniformLocation(gl, "worldMapTexture"), 0);
      CheckError();
    }


    private void LoadShaders()
    {
      ShaderProgram = new ShaderProgram();
      CheckError();

      ShaderProgram.Create(gl,
        Encoding.ASCII.GetString(Properties.Resources.EarthVertexShader),
        Encoding.ASCII.GetString(Properties.Resources.EarthFragmentShader),
        null);
      CheckError();

      ShaderProgram.AssertValid(gl);
      CheckError();

      ShaderProgram.Bind(gl);
      CheckError();
    }


    private const int PHI_STEPS = 192;
    private const int RO_STEPS = 96;
    private const float TwoPi = (float)(2 * Math.PI);

    private void CreateVba()
    {
      CheckError(false);

      VertexBufferArray = new VertexBufferArray();
      CheckError();
      VertexBufferArray.Create(gl);
      CheckError();
      VertexBufferArray.Bind(gl);
      CheckError();

      // generate a grid of polar coordinates
      VertexCount = RO_STEPS * (PHI_STEPS + 1) * 2;
      var data = new float[VertexCount * 2];
      int idx = 0;

      for (int ro = 0; ro < RO_STEPS; ro++)
        for (int phi = 0; phi <= PHI_STEPS; phi++)
        {
          data[idx++] = ro / (float)RO_STEPS;
          data[idx++] = phi * TwoPi / PHI_STEPS;
          data[idx++] = (ro + 1) / (float)RO_STEPS;
          data[idx++] = phi * TwoPi / PHI_STEPS;
        }

      var dataBuffer = new VertexBuffer();
      CheckError();
      dataBuffer.Create(gl);
      CheckError();
      dataBuffer.Bind(gl);
      CheckError();
      dataBuffer.SetData(gl, 0, data, false, 2);
      CheckError();

      VertexBufferArray.Unbind(gl);
      CheckError();
    }

    private void LoadTexture()
    {
      CheckError(false);

      gl.GenTextures(1, TextureIds);
      CheckError();

      gl.BindTexture(OpenGL.GL_TEXTURE_2D, TextureIds[0]);
      CheckError(false); // produces false positives

      gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, OpenGL.GL_LINEAR);    // shrink
      gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, OpenGL.GL_LINEAR);    // stretch
      gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_T, OpenGL.GL_CLAMP_TO_EDGE); // lat
      gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_S, OpenGL.GL_REPEAT);        // lon


      //using (Bitmap bitmap = new Bitmap(@"C:\Users\Alex\Desktop\sat\maps\worldMapTexture.bmp"))
      //using (Bitmap bitmap = new Bitmap(Properties.Resources.NaturalEarth))
      using (Bitmap bitmap = new Bitmap(Properties.Resources.DxAtlasMap1))
      {
        BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
          ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

        gl.TexImage2D(OpenGL.GL_TEXTURE_2D, 0, OpenGL.GL_RGB, bitmap.Width, bitmap.Height, 0,
          OpenGL.GL_BGR, OpenGL.GL_UNSIGNED_BYTE, data.Scan0);

        bitmap.UnlockBits(data);
      }
      CheckError();
    }




    //----------------------------------------------------------------------------------------------
    //                                        draw
    //----------------------------------------------------------------------------------------------
    private void openglControl1_OpenGLDraw(object sender, SharpGL.RenderEventArgs args)
    {
      var gl = openglControl1.OpenGL;
      CheckError(false);

      ShaderProgram.Bind(gl);
      CheckError();

      gl.ActiveTexture(OpenGL.GL_TEXTURE0);
      CheckError();
      gl.BindTexture(OpenGL.GL_TEXTURE_2D, TextureIds[0]);
      CheckError();

      VertexBufferArray.Bind(gl);
      CheckError();

      ShaderProgram.SetUniform1(gl, "in_homeLat", (float)Center.LatitudeRad);
      CheckError();
      ShaderProgram.SetUniform1(gl, "in_homeLon", (float)Center.LongitudeRad);
      CheckError();
      ShaderProgram.SetUniform1(gl, "in_footprint", (float)Footprint);
      CheckError();
      ShaderProgram.SetUniform1(gl, "in_aspect", openglControl1.ClientSize.Width / (float)openglControl1.ClientSize.Height);
      CheckError();
      ShaderProgram.SetUniform1(gl, "in_zoom", (float)Zoom);
      CheckError();

      gl.ClearColor(0.7f, 0.7f, 0.7f, 1);
      CheckError();
      gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT);
      CheckError();
      gl.DrawArrays(OpenGL.GL_TRIANGLE_STRIP, 0, VertexCount);
      CheckError();

      VertexBufferArray.Unbind(gl);
      CheckError();
      ShaderProgram.Unbind(gl);
      CheckError();

      SetSpriteAttributes();
      SpriteRenderer.DrawSprites([SatelliteSprite, HomeSprite]);
      CheckError();
    }

    private void SetSpriteAttributes()
    {
      // control size, pixels
      var size = openglControl1.ClientSize;
      // diameter of the inscribed circle, pixels
      var diam = Math.Min(size.Width, size.Height);
      
      // scale sprites to look nice at all control sizes
      var scale = 0.6f + diam * 0.0007f;
      SatelliteSprite.Scale = scale;
      HomeSprite.Scale = scale;

      // satellite points in the direction of the next position
      SatelliteSprite.Angle = (float)(-Azimuth + Math.PI / 4);

      // home location in the azimuthal projection
      var path = Home - Center;
      var ro = path.DistanceRad / Math.PI * Zoom;
      var phi = Geo.HalfPi - path.AzimuthRad;

      HomeSprite.Location = new PointF(
        (float)(ro * Math.Cos(phi) * diam / size.Width),
        (float)(ro * Math.Sin(phi) * diam / size.Height));
    }



    //----------------------------------------------------------------------------------------------
    //                                   error handling
    //----------------------------------------------------------------------------------------------
    private void CheckError(bool log = true)
    {
      uint err;

      while ((err = gl.GetError()) != OpenGL.GL_NO_ERROR)
        if (log)
        {
          string stackTrace = new StackTrace(true).ToString();
          if (IsLoggableError(stackTrace))
            Log.Error($"{gl.ErrorString(err)}\n{stackTrace}");
        }
    }

    // GL scene may be updated many times per second.
    // If an error occurs, do not log it every time
    private DateTime NextLogErrortime = DateTime.MinValue;

    private bool IsLoggableError(string stackTrace)
    {
      return true;

      // if (!stackTrace.Contains("AppendSpectrum")) return true;
      // 
      // if (DateTime.UtcNow < NextLogErrortime) return false;
      // NextLogErrortime = DateTime.UtcNow + TimeSpan.FromSeconds(30);
      // return true;
    }

    private void SatelliteRadioBtn_CheckedChanged(object sender, EventArgs e)
    {
      if (!((RadioButton)sender).Checked) return;

      SatelliteRadioBtn.Font = SatelliteRadioBtn.Checked ? BoldFont : RegularFont;
      OrbitRadioBtn.Font = OrbitRadioBtn.Checked ? BoldFont : RegularFont;
    }
  }
}

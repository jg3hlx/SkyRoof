using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;
using System.Numerics;
using System.Runtime;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using SharpGL;
using SharpGL.Shaders;
using SharpGL.VertexBuffers;

namespace OrbiNom
{
  public class Sprite
  {
    private uint[] TextureIds = new uint[1];
    private OpenGL gl;
    private Size ImageSize;
    private float scale = 0.7f;

    public PointF Location;
    public float Angle;
    public float Scale { get => scale; set => SetScale(value); }


    public uint TextureId => TextureIds[0];
    public Matrix4x4 Transform => GetTransform();

    public Sprite(OpenGL gl, byte[] imageBytes)
    {
      this.gl = gl;
      LoadTexture(imageBytes);
    }

    private void LoadTexture(byte[] imageBytes)
    {
      gl.ActiveTexture(OpenGL.GL_TEXTURE0);

      gl.GenTextures(1, TextureIds);
      gl.BindTexture(OpenGL.GL_TEXTURE_2D, TextureId);

      gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MIN_FILTER, OpenGL.GL_LINEAR);    // shrink
      gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_MAG_FILTER, OpenGL.GL_LINEAR);    // stretch
      gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_T, OpenGL.GL_CLAMP_TO_EDGE); // lat
      gl.TexParameter(OpenGL.GL_TEXTURE_2D, OpenGL.GL_TEXTURE_WRAP_S, OpenGL.GL_CLAMP_TO_EDGE);        // lon

      using (Bitmap bitmap = BitmapFromBytes(imageBytes))
      {
        ImageSize = bitmap.Size;

        BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
        ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

        gl.TexImage2D(OpenGL.GL_TEXTURE_2D, 0, OpenGL.GL_RGBA, bitmap.Width, bitmap.Height, 0,
          OpenGL.GL_BGRA, OpenGL.GL_UNSIGNED_BYTE, data.Scan0);

        bitmap.UnlockBits(data);
      }      
    }  
    
    private Bitmap BitmapFromBytes(byte[] imageBytes)
    {
      using (MemoryStream ms = new MemoryStream(imageBytes))
        return new Bitmap(Image.FromStream(ms));
    }

    private void SetScale(float value) 
    { 
      scale = Math.Max(0.25f, Math.Min(1, value)); 
    }

    private Matrix4x4 GetTransform()
    {
      // 0, 0, w, h
      var viewport = new int[4];
      gl.GetInteger(OpenGL.GL_VIEWPORT, viewport);

      float scaleX = Scale * ImageSize.Width / (float)viewport[2];
      float scaleY = Scale * ImageSize.Height / (float)viewport[3];



      return
        Matrix4x4.CreateTranslation(Location.X, Location.Y, 0) *
        Matrix4x4.CreateRotationZ(Angle) *
        Matrix4x4.CreateScale(Scale * scaleX, Scale * scaleY, 1);
    }
  }
}

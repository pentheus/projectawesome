using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameEditor
{
	class ReferenceGrid
	{
		// Public settings
		public int lineCount = 50;
		public int squareSize = 100;
		public Color color = Color.Green;

		List<VertexPositionColor[]> vertices = new List<VertexPositionColor[]>();
		Matrix world = Matrix.Identity;
		GraphicsDevice device;
		BasicEffect effect;

		public ReferenceGrid (GraphicsDevice grphDevice) : this(grphDevice, 50, 100, Color.Green)
		{

		}

		public ReferenceGrid (GraphicsDevice device, int lineCount, int squareSize, Color color)
		{
			this.device = device;
			this.lineCount = lineCount;
			this.squareSize = squareSize;
			this.color = color;

			effect = new BasicEffect(device, null);

			SetupEffect();
			SetupVertices();
		}

		int lineLength
		{
			get
			{
				return ( lineCount * squareSize );
			}
		}

		Vector3 startPos
		{
			get
			{
				Vector3 pos = Vector3.Zero;
				pos.X -= lineLength / 2;
				pos.Z -= lineLength / 2;
				return pos;
			}

		}

		void SetupVertices ()
		{
			Vector3 pos = startPos;
			for (int i = ( lineCount - 1 ); i > 0; --i)
			{
				pos.X += squareSize;
				vertices.Add(new VertexPositionColor[] {
					new VertexPositionColor(pos + (Vector3.UnitZ*lineLength), color),
					new VertexPositionColor(pos, color)
				});
			}

			pos = startPos;
			for (int i = ( lineCount - 1 ); i > 0; --i)
			{
				pos.Z += squareSize;
				vertices.Add(new VertexPositionColor[] {
					new VertexPositionColor(pos + (Vector3.UnitX*lineLength), color),
					new VertexPositionColor(pos, color)
				});
			}
		}

		void SetupEffect ()
		{
		}

		public void Draw (Matrix view, Matrix projection)
		{
			device.VertexDeclaration =
				new VertexDeclaration(device,
					VertexPositionColor.VertexElements);

			effect.World = Matrix.CreateTranslation(Vector3.Zero);
			effect.View = view;
			effect.Projection = projection;
			effect.VertexColorEnabled = true;

			effect.Begin();

			foreach (VertexPositionColor[] line in vertices)
			{
				foreach (EffectPass pass in effect.CurrentTechnique.Passes)
				{
					pass.Begin();
					device.DrawUserPrimitives<VertexPositionColor>
						(PrimitiveType.LineStrip, line, 0, 1);
					pass.End();
				}
			}

			effect.End();
		}

	}
}

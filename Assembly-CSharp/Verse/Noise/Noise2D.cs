using System;
using System.Xml.Serialization;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000768 RID: 1896
	public class Noise2D : IDisposable
	{
		// Token: 0x06003BD6 RID: 15318 RVA: 0x0017BD72 File Offset: 0x00179F72
		protected Noise2D()
		{
		}

		// Token: 0x06003BD7 RID: 15319 RVA: 0x0017BD8C File Offset: 0x00179F8C
		public Noise2D(int size) : this(size, size, null)
		{
		}

		// Token: 0x06003BD8 RID: 15320 RVA: 0x0017BD97 File Offset: 0x00179F97
		public Noise2D(int size, ModuleBase generator) : this(size, size, generator)
		{
		}

		// Token: 0x06003BD9 RID: 15321 RVA: 0x0017BDA2 File Offset: 0x00179FA2
		public Noise2D(int width, int height) : this(width, height, null)
		{
		}

		// Token: 0x06003BDA RID: 15322 RVA: 0x0017BDB0 File Offset: 0x00179FB0
		public Noise2D(int width, int height, ModuleBase generator)
		{
			this.m_generator = generator;
			this.m_width = width;
			this.m_height = height;
			this.m_data = new float[width, height];
			this.m_ucWidth = width + this.m_ucBorder * 2;
			this.m_ucHeight = height + this.m_ucBorder * 2;
			this.m_ucData = new float[width + this.m_ucBorder * 2, height + this.m_ucBorder * 2];
		}

		// Token: 0x17000BAC RID: 2988
		public float this[int x, int y, bool isCropped = true]
		{
			get
			{
				if (isCropped)
				{
					if (x < 0 && x >= this.m_width)
					{
						throw new ArgumentOutOfRangeException("Invalid x position");
					}
					if (y < 0 && y >= this.m_height)
					{
						throw new ArgumentOutOfRangeException("Inavlid y position");
					}
					return this.m_data[x, y];
				}
				else
				{
					if (x < 0 && x >= this.m_ucWidth)
					{
						throw new ArgumentOutOfRangeException("Invalid x position");
					}
					if (y < 0 && y >= this.m_ucHeight)
					{
						throw new ArgumentOutOfRangeException("Inavlid y position");
					}
					return this.m_ucData[x, y];
				}
			}
			set
			{
				if (isCropped)
				{
					if (x < 0 && x >= this.m_width)
					{
						throw new ArgumentOutOfRangeException("Invalid x position");
					}
					if (y < 0 && y >= this.m_height)
					{
						throw new ArgumentOutOfRangeException("Invalid y position");
					}
					this.m_data[x, y] = value;
					return;
				}
				else
				{
					if (x < 0 && x >= this.m_ucWidth)
					{
						throw new ArgumentOutOfRangeException("Invalid x position");
					}
					if (y < 0 && y >= this.m_ucHeight)
					{
						throw new ArgumentOutOfRangeException("Inavlid y position");
					}
					this.m_ucData[x, y] = value;
					return;
				}
			}
		}

		// Token: 0x17000BAD RID: 2989
		// (get) Token: 0x06003BDD RID: 15325 RVA: 0x0017BF53 File Offset: 0x0017A153
		// (set) Token: 0x06003BDE RID: 15326 RVA: 0x0017BF5B File Offset: 0x0017A15B
		public float Border
		{
			get
			{
				return this.m_borderValue;
			}
			set
			{
				this.m_borderValue = value;
			}
		}

		// Token: 0x17000BAE RID: 2990
		// (get) Token: 0x06003BDF RID: 15327 RVA: 0x0017BF64 File Offset: 0x0017A164
		// (set) Token: 0x06003BE0 RID: 15328 RVA: 0x0017BF6C File Offset: 0x0017A16C
		public ModuleBase Generator
		{
			get
			{
				return this.m_generator;
			}
			set
			{
				this.m_generator = value;
			}
		}

		// Token: 0x17000BAF RID: 2991
		// (get) Token: 0x06003BE1 RID: 15329 RVA: 0x0017BF75 File Offset: 0x0017A175
		public int Height
		{
			get
			{
				return this.m_height;
			}
		}

		// Token: 0x17000BB0 RID: 2992
		// (get) Token: 0x06003BE2 RID: 15330 RVA: 0x0017BF7D File Offset: 0x0017A17D
		public int Width
		{
			get
			{
				return this.m_width;
			}
		}

		// Token: 0x06003BE3 RID: 15331 RVA: 0x0017BF85 File Offset: 0x0017A185
		public float[,] GetNormalizedData(bool isCropped = true, int xCrop = 0, int yCrop = 0)
		{
			return this.GetData(isCropped, xCrop, yCrop, true);
		}

		// Token: 0x06003BE4 RID: 15332 RVA: 0x0017BF94 File Offset: 0x0017A194
		public float[,] GetData(bool isCropped = true, int xCrop = 0, int yCrop = 0, bool isNormalized = false)
		{
			int num;
			int num2;
			float[,] array;
			if (isCropped)
			{
				num = this.m_width;
				num2 = this.m_height;
				array = this.m_data;
			}
			else
			{
				num = this.m_ucWidth;
				num2 = this.m_ucHeight;
				array = this.m_ucData;
			}
			num -= xCrop;
			num2 -= yCrop;
			float[,] array2 = new float[num, num2];
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num2; j++)
				{
					float num3;
					if (isNormalized)
					{
						num3 = (array[i, j] + 1f) / 2f;
					}
					else
					{
						num3 = array[i, j];
					}
					array2[i, j] = num3;
				}
			}
			return array2;
		}

		// Token: 0x06003BE5 RID: 15333 RVA: 0x0017C039 File Offset: 0x0017A239
		public void Clear()
		{
			this.Clear(0f);
		}

		// Token: 0x06003BE6 RID: 15334 RVA: 0x0017C048 File Offset: 0x0017A248
		public void Clear(float value)
		{
			for (int i = 0; i < this.m_width; i++)
			{
				for (int j = 0; j < this.m_height; j++)
				{
					this.m_data[i, j] = value;
				}
			}
		}

		// Token: 0x06003BE7 RID: 15335 RVA: 0x0017C085 File Offset: 0x0017A285
		private double GeneratePlanar(double x, double y)
		{
			return this.m_generator.GetValue(x, 0.0, y);
		}

		// Token: 0x06003BE8 RID: 15336 RVA: 0x0017C09D File Offset: 0x0017A29D
		public void GeneratePlanar(double left, double right, double top, double bottom)
		{
			this.GeneratePlanar(left, right, top, bottom, true);
		}

		// Token: 0x06003BE9 RID: 15337 RVA: 0x0017C0AC File Offset: 0x0017A2AC
		public void GeneratePlanar(double left, double right, double top, double bottom, bool isSeamless)
		{
			if (right <= left || bottom <= top)
			{
				throw new ArgumentException("Invalid right/left or bottom/top combination");
			}
			if (this.m_generator == null)
			{
				throw new ArgumentNullException("Generator is null");
			}
			double num = right - left;
			double num2 = bottom - top;
			double num3 = num / ((double)this.m_width - (double)this.m_ucBorder);
			double num4 = num2 / ((double)this.m_height - (double)this.m_ucBorder);
			double num5 = left;
			for (int i = 0; i < this.m_ucWidth; i++)
			{
				double num6 = top;
				for (int j = 0; j < this.m_ucHeight; j++)
				{
					float num7;
					if (isSeamless)
					{
						num7 = (float)this.GeneratePlanar(num5, num6);
					}
					else
					{
						double a = this.GeneratePlanar(num5, num6);
						double b = this.GeneratePlanar(num5 + num, num6);
						double a2 = this.GeneratePlanar(num5, num6 + num2);
						double b2 = this.GeneratePlanar(num5 + num, num6 + num2);
						double position = 1.0 - (num5 - left) / num;
						double position2 = 1.0 - (num6 - top) / num2;
						double a3 = Utils.InterpolateLinear(a, b, position);
						double b3 = Utils.InterpolateLinear(a2, b2, position);
						num7 = (float)Utils.InterpolateLinear(a3, b3, position2);
					}
					this.m_ucData[i, j] = num7;
					if (i >= this.m_ucBorder && j >= this.m_ucBorder && i < this.m_width + this.m_ucBorder && j < this.m_height + this.m_ucBorder)
					{
						this.m_data[i - this.m_ucBorder, j - this.m_ucBorder] = num7;
					}
					num6 += num4;
				}
				num5 += num3;
			}
		}

		// Token: 0x06003BEA RID: 15338 RVA: 0x0017C254 File Offset: 0x0017A454
		private double GenerateCylindrical(double angle, double height)
		{
			double x = Math.Cos(angle * 0.017453292519943295);
			double z = Math.Sin(angle * 0.017453292519943295);
			return this.m_generator.GetValue(x, height, z);
		}

		// Token: 0x06003BEB RID: 15339 RVA: 0x0017C294 File Offset: 0x0017A494
		public void GenerateCylindrical(double angleMin, double angleMax, double heightMin, double heightMax)
		{
			if (angleMax <= angleMin || heightMax <= heightMin)
			{
				throw new ArgumentException("Invalid angle or height parameters");
			}
			if (this.m_generator == null)
			{
				throw new ArgumentNullException("Generator is null");
			}
			double num = angleMax - angleMin;
			double num2 = heightMax - heightMin;
			double num3 = num / ((double)this.m_width - (double)this.m_ucBorder);
			double num4 = num2 / ((double)this.m_height - (double)this.m_ucBorder);
			double num5 = angleMin;
			for (int i = 0; i < this.m_ucWidth; i++)
			{
				double num6 = heightMin;
				for (int j = 0; j < this.m_ucHeight; j++)
				{
					this.m_ucData[i, j] = (float)this.GenerateCylindrical(num5, num6);
					if (i >= this.m_ucBorder && j >= this.m_ucBorder && i < this.m_width + this.m_ucBorder && j < this.m_height + this.m_ucBorder)
					{
						this.m_data[i - this.m_ucBorder, j - this.m_ucBorder] = (float)this.GenerateCylindrical(num5, num6);
					}
					num6 += num4;
				}
				num5 += num3;
			}
		}

		// Token: 0x06003BEC RID: 15340 RVA: 0x0017C3B4 File Offset: 0x0017A5B4
		private double GenerateSpherical(double lat, double lon)
		{
			double num = Math.Cos(0.017453292519943295 * lat);
			return this.m_generator.GetValue(num * Math.Cos(0.017453292519943295 * lon), Math.Sin(0.017453292519943295 * lat), num * Math.Sin(0.017453292519943295 * lon));
		}

		// Token: 0x06003BED RID: 15341 RVA: 0x0017C414 File Offset: 0x0017A614
		public void GenerateSpherical(double south, double north, double west, double east)
		{
			if (east <= west || north <= south)
			{
				throw new ArgumentException("Invalid east/west or north/south combination");
			}
			if (this.m_generator == null)
			{
				throw new ArgumentNullException("Generator is null");
			}
			double num = east - west;
			double num2 = north - south;
			double num3 = num / ((double)this.m_width - (double)this.m_ucBorder);
			double num4 = num2 / ((double)this.m_height - (double)this.m_ucBorder);
			double num5 = west;
			for (int i = 0; i < this.m_ucWidth; i++)
			{
				double num6 = south;
				for (int j = 0; j < this.m_ucHeight; j++)
				{
					this.m_ucData[i, j] = (float)this.GenerateSpherical(num6, num5);
					if (i >= this.m_ucBorder && j >= this.m_ucBorder && i < this.m_width + this.m_ucBorder && j < this.m_height + this.m_ucBorder)
					{
						this.m_data[i - this.m_ucBorder, j - this.m_ucBorder] = (float)this.GenerateSpherical(num6, num5);
					}
					num6 += num4;
				}
				num5 += num3;
			}
		}

		// Token: 0x06003BEE RID: 15342 RVA: 0x0017C531 File Offset: 0x0017A731
		public Texture2D GetTexture()
		{
			return this.GetTexture(GradientPresets.Grayscale);
		}

		// Token: 0x06003BEF RID: 15343 RVA: 0x0017C540 File Offset: 0x0017A740
		public Texture2D GetTexture(Gradient gradient)
		{
			Texture2D texture2D = new Texture2D(this.m_width, this.m_height);
			texture2D.name = "Noise2DTex";
			Color[] array = new Color[this.m_width * this.m_height];
			for (int i = 0; i < this.m_width; i++)
			{
				for (int j = 0; j < this.m_height; j++)
				{
					float num;
					if (!float.IsNaN(this.m_borderValue) && (i == 0 || i == this.m_width - this.m_ucBorder || j == 0 || j == this.m_height - this.m_ucBorder))
					{
						num = this.m_borderValue;
					}
					else
					{
						num = this.m_data[i, j];
					}
					array[i + j * this.m_width] = gradient.Evaluate((num + 1f) / 2f);
				}
			}
			texture2D.SetPixels(array);
			texture2D.wrapMode = TextureWrapMode.Clamp;
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x06003BF0 RID: 15344 RVA: 0x0017C634 File Offset: 0x0017A834
		public Texture2D GetNormalMap(float intensity)
		{
			Texture2D texture2D = new Texture2D(this.m_width, this.m_height);
			texture2D.name = "Noise2DTex";
			Color[] array = new Color[this.m_width * this.m_height];
			for (int i = 0; i < this.m_ucWidth; i++)
			{
				for (int j = 0; j < this.m_ucHeight; j++)
				{
					float num = (this.m_ucData[Mathf.Max(0, i - this.m_ucBorder), j] - this.m_ucData[Mathf.Min(i + this.m_ucBorder, this.m_height + this.m_ucBorder), j]) / 2f;
					float num2 = (this.m_ucData[i, Mathf.Max(0, j - this.m_ucBorder)] - this.m_ucData[i, Mathf.Min(j + this.m_ucBorder, this.m_width + this.m_ucBorder)]) / 2f;
					Vector3 a = new Vector3(num * intensity, 0f, 1f);
					Vector3 b = new Vector3(0f, num2 * intensity, 1f);
					Vector3 vector = a + b;
					vector.Normalize();
					Vector3 zero = Vector3.zero;
					zero.x = (vector.x + 1f) / 2f;
					zero.y = (vector.y + 1f) / 2f;
					zero.z = (vector.z + 1f) / 2f;
					if (i >= this.m_ucBorder && j >= this.m_ucBorder && i < this.m_width + this.m_ucBorder && j < this.m_height + this.m_ucBorder)
					{
						array[i - this.m_ucBorder + (j - this.m_ucBorder) * this.m_width] = new Color(zero.x, zero.y, zero.z);
					}
				}
			}
			texture2D.SetPixels(array);
			texture2D.wrapMode = TextureWrapMode.Clamp;
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x17000BB1 RID: 2993
		// (get) Token: 0x06003BF1 RID: 15345 RVA: 0x0017C83D File Offset: 0x0017AA3D
		public bool IsDisposed
		{
			get
			{
				return this.m_disposed;
			}
		}

		// Token: 0x06003BF2 RID: 15346 RVA: 0x0017C845 File Offset: 0x0017AA45
		public void Dispose()
		{
			if (!this.m_disposed)
			{
				this.m_disposed = this.Disposing();
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x06003BF3 RID: 15347 RVA: 0x0017C861 File Offset: 0x0017AA61
		protected virtual bool Disposing()
		{
			if (this.m_data != null)
			{
				this.m_data = null;
			}
			this.m_width = 0;
			this.m_height = 0;
			return true;
		}

		// Token: 0x04002717 RID: 10007
		public static readonly double South = -90.0;

		// Token: 0x04002718 RID: 10008
		public static readonly double North = 90.0;

		// Token: 0x04002719 RID: 10009
		public static readonly double West = -180.0;

		// Token: 0x0400271A RID: 10010
		public static readonly double East = 180.0;

		// Token: 0x0400271B RID: 10011
		public static readonly double AngleMin = -180.0;

		// Token: 0x0400271C RID: 10012
		public static readonly double AngleMax = 180.0;

		// Token: 0x0400271D RID: 10013
		public static readonly double Left = -1.0;

		// Token: 0x0400271E RID: 10014
		public static readonly double Right = 1.0;

		// Token: 0x0400271F RID: 10015
		public static readonly double Top = -1.0;

		// Token: 0x04002720 RID: 10016
		public static readonly double Bottom = 1.0;

		// Token: 0x04002721 RID: 10017
		private int m_width;

		// Token: 0x04002722 RID: 10018
		private int m_height;

		// Token: 0x04002723 RID: 10019
		private float[,] m_data;

		// Token: 0x04002724 RID: 10020
		private int m_ucWidth;

		// Token: 0x04002725 RID: 10021
		private int m_ucHeight;

		// Token: 0x04002726 RID: 10022
		private int m_ucBorder = 1;

		// Token: 0x04002727 RID: 10023
		private float[,] m_ucData;

		// Token: 0x04002728 RID: 10024
		private float m_borderValue = float.NaN;

		// Token: 0x04002729 RID: 10025
		private ModuleBase m_generator;

		// Token: 0x0400272A RID: 10026
		[XmlIgnore]
		[NonSerialized]
		private bool m_disposed;
	}
}

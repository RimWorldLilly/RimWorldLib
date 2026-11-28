using System;

namespace Verse.Noise
{
	// Token: 0x0200077B RID: 1915
	public class Rotate : ModuleBase
	{
		// Token: 0x06003C42 RID: 15426 RVA: 0x0017D2ED File Offset: 0x0017B4ED
		public Rotate() : base(1)
		{
			this.SetAngles(0.0, 0.0, 0.0);
		}

		// Token: 0x06003C43 RID: 15427 RVA: 0x0017D317 File Offset: 0x0017B517
		public Rotate(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06003C44 RID: 15428 RVA: 0x0017D329 File Offset: 0x0017B529
		public Rotate(double x, double y, double z, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.SetAngles(x, y, z);
		}

		// Token: 0x17000BBC RID: 3004
		// (get) Token: 0x06003C45 RID: 15429 RVA: 0x0017D345 File Offset: 0x0017B545
		// (set) Token: 0x06003C46 RID: 15430 RVA: 0x0017D34D File Offset: 0x0017B54D
		public double X
		{
			get
			{
				return this.m_x;
			}
			set
			{
				this.SetAngles(value, this.m_y, this.m_z);
			}
		}

		// Token: 0x17000BBD RID: 3005
		// (get) Token: 0x06003C47 RID: 15431 RVA: 0x0017D362 File Offset: 0x0017B562
		// (set) Token: 0x06003C48 RID: 15432 RVA: 0x0017D36A File Offset: 0x0017B56A
		public double Y
		{
			get
			{
				return this.m_y;
			}
			set
			{
				this.SetAngles(this.m_x, value, this.m_z);
			}
		}

		// Token: 0x17000BBE RID: 3006
		// (get) Token: 0x06003C49 RID: 15433 RVA: 0x0017D37F File Offset: 0x0017B57F
		// (set) Token: 0x06003C4A RID: 15434 RVA: 0x0017D387 File Offset: 0x0017B587
		public double Z
		{
			get
			{
				return this.m_x;
			}
			set
			{
				this.SetAngles(this.m_x, this.m_y, value);
			}
		}

		// Token: 0x06003C4B RID: 15435 RVA: 0x0017D39C File Offset: 0x0017B59C
		private void SetAngles(double x, double y, double z)
		{
			double num = Math.Cos(x * 0.017453292519943295);
			double num2 = Math.Cos(y * 0.017453292519943295);
			double num3 = Math.Cos(z * 0.017453292519943295);
			double num4 = Math.Sin(x * 0.017453292519943295);
			double num5 = Math.Sin(y * 0.017453292519943295);
			double num6 = Math.Sin(z * 0.017453292519943295);
			this.m_x1Matrix = num5 * num4 * num6 + num2 * num3;
			this.m_y1Matrix = num * num6;
			this.m_z1Matrix = num5 * num3 - num2 * num4 * num6;
			this.m_x2Matrix = num5 * num4 * num3 - num2 * num6;
			this.m_y2Matrix = num * num3;
			this.m_z2Matrix = -num2 * num4 * num3 - num5 * num6;
			this.m_x3Matrix = -num5 * num;
			this.m_y3Matrix = num4;
			this.m_z3Matrix = num2 * num;
			this.m_x = x;
			this.m_y = y;
			this.m_z = z;
		}

		// Token: 0x06003C4C RID: 15436 RVA: 0x0017D49C File Offset: 0x0017B69C
		public override double GetValue(double x, double y, double z)
		{
			double x2 = this.m_x1Matrix * x + this.m_y1Matrix * y + this.m_z1Matrix * z;
			double y2 = this.m_x2Matrix * x + this.m_y2Matrix * y + this.m_z2Matrix * z;
			double z2 = this.m_x3Matrix * x + this.m_y3Matrix * y + this.m_z3Matrix * z;
			return this.modules[0].GetValue(x2, y2, z2);
		}

		// Token: 0x04002739 RID: 10041
		private double m_x;

		// Token: 0x0400273A RID: 10042
		private double m_x1Matrix;

		// Token: 0x0400273B RID: 10043
		private double m_x2Matrix;

		// Token: 0x0400273C RID: 10044
		private double m_x3Matrix;

		// Token: 0x0400273D RID: 10045
		private double m_y;

		// Token: 0x0400273E RID: 10046
		private double m_y1Matrix;

		// Token: 0x0400273F RID: 10047
		private double m_y2Matrix;

		// Token: 0x04002740 RID: 10048
		private double m_y3Matrix;

		// Token: 0x04002741 RID: 10049
		private double m_z;

		// Token: 0x04002742 RID: 10050
		private double m_z1Matrix;

		// Token: 0x04002743 RID: 10051
		private double m_z2Matrix;

		// Token: 0x04002744 RID: 10052
		private double m_z3Matrix;
	}
}

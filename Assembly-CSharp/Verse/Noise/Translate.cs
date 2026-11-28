using System;

namespace Verse.Noise
{
	// Token: 0x02000782 RID: 1922
	public class Translate : ModuleBase
	{
		// Token: 0x06003C7D RID: 15485 RVA: 0x0017DC8C File Offset: 0x0017BE8C
		public Translate() : base(1)
		{
		}

		// Token: 0x06003C7E RID: 15486 RVA: 0x0017DCC2 File Offset: 0x0017BEC2
		public Translate(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06003C7F RID: 15487 RVA: 0x0017DD04 File Offset: 0x0017BF04
		public Translate(double x, double y, double z, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		// Token: 0x17000BCB RID: 3019
		// (get) Token: 0x06003C80 RID: 15488 RVA: 0x0017DD64 File Offset: 0x0017BF64
		// (set) Token: 0x06003C81 RID: 15489 RVA: 0x0017DD6C File Offset: 0x0017BF6C
		public double X
		{
			get
			{
				return this.m_x;
			}
			set
			{
				this.m_x = value;
			}
		}

		// Token: 0x17000BCC RID: 3020
		// (get) Token: 0x06003C82 RID: 15490 RVA: 0x0017DD75 File Offset: 0x0017BF75
		// (set) Token: 0x06003C83 RID: 15491 RVA: 0x0017DD7D File Offset: 0x0017BF7D
		public double Y
		{
			get
			{
				return this.m_y;
			}
			set
			{
				this.m_y = value;
			}
		}

		// Token: 0x17000BCD RID: 3021
		// (get) Token: 0x06003C84 RID: 15492 RVA: 0x0017DD86 File Offset: 0x0017BF86
		// (set) Token: 0x06003C85 RID: 15493 RVA: 0x0017DD8E File Offset: 0x0017BF8E
		public double Z
		{
			get
			{
				return this.m_z;
			}
			set
			{
				this.m_z = value;
			}
		}

		// Token: 0x06003C86 RID: 15494 RVA: 0x0017DD97 File Offset: 0x0017BF97
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x + this.m_x, y + this.m_y, z + this.m_z);
		}

		// Token: 0x04002751 RID: 10065
		private double m_x = 1.0;

		// Token: 0x04002752 RID: 10066
		private double m_y = 1.0;

		// Token: 0x04002753 RID: 10067
		private double m_z = 1.0;
	}
}

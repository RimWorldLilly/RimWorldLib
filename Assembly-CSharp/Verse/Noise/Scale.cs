using System;

namespace Verse.Noise
{
	// Token: 0x0200077C RID: 1916
	public class Scale : ModuleBase
	{
		// Token: 0x06003C4D RID: 15437 RVA: 0x0017D50A File Offset: 0x0017B70A
		public Scale() : base(1)
		{
		}

		// Token: 0x06003C4E RID: 15438 RVA: 0x0017D540 File Offset: 0x0017B740
		public Scale(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06003C4F RID: 15439 RVA: 0x0017D580 File Offset: 0x0017B780
		public Scale(double x, double y, double z, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		// Token: 0x17000BBF RID: 3007
		// (get) Token: 0x06003C50 RID: 15440 RVA: 0x0017D5E0 File Offset: 0x0017B7E0
		// (set) Token: 0x06003C51 RID: 15441 RVA: 0x0017D5E8 File Offset: 0x0017B7E8
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

		// Token: 0x17000BC0 RID: 3008
		// (get) Token: 0x06003C52 RID: 15442 RVA: 0x0017D5F1 File Offset: 0x0017B7F1
		// (set) Token: 0x06003C53 RID: 15443 RVA: 0x0017D5F9 File Offset: 0x0017B7F9
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

		// Token: 0x17000BC1 RID: 3009
		// (get) Token: 0x06003C54 RID: 15444 RVA: 0x0017D602 File Offset: 0x0017B802
		// (set) Token: 0x06003C55 RID: 15445 RVA: 0x0017D60A File Offset: 0x0017B80A
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

		// Token: 0x06003C56 RID: 15446 RVA: 0x0017D613 File Offset: 0x0017B813
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x * this.m_x, y * this.m_y, z * this.m_z);
		}

		// Token: 0x04002745 RID: 10053
		private double m_x = 1.0;

		// Token: 0x04002746 RID: 10054
		private double m_y = 1.0;

		// Token: 0x04002747 RID: 10055
		private double m_z = 1.0;
	}
}

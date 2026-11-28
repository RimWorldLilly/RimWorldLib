using System;

namespace Verse.Noise
{
	// Token: 0x0200076D RID: 1901
	public class Cache : ModuleBase
	{
		// Token: 0x06003C03 RID: 15363 RVA: 0x0017CA72 File Offset: 0x0017AC72
		public Cache() : base(1)
		{
		}

		// Token: 0x06003C04 RID: 15364 RVA: 0x0017CA7B File Offset: 0x0017AC7B
		public Cache(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x17000BB3 RID: 2995
		public override ModuleBase this[int index]
		{
			get
			{
				return base[index];
			}
			set
			{
				base[index] = value;
				this.m_cached = false;
			}
		}

		// Token: 0x06003C07 RID: 15367 RVA: 0x0017CAA8 File Offset: 0x0017ACA8
		public override double GetValue(double x, double y, double z)
		{
			if (!this.m_cached || this.m_x != x || this.m_y != y || this.m_z != z)
			{
				this.m_value = this.modules[0].GetValue(x, y, z);
				this.m_x = x;
				this.m_y = y;
				this.m_z = z;
			}
			this.m_cached = true;
			return this.m_value;
		}

		// Token: 0x0400272C RID: 10028
		private double m_value;

		// Token: 0x0400272D RID: 10029
		private bool m_cached;

		// Token: 0x0400272E RID: 10030
		private double m_x;

		// Token: 0x0400272F RID: 10031
		private double m_y;

		// Token: 0x04002730 RID: 10032
		private double m_z;
	}
}

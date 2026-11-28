using System;

namespace Verse.Noise
{
	// Token: 0x02000770 RID: 1904
	public class Displace : ModuleBase
	{
		// Token: 0x06003C18 RID: 15384 RVA: 0x0017CE4B File Offset: 0x0017B04B
		public Displace() : base(4)
		{
		}

		// Token: 0x06003C19 RID: 15385 RVA: 0x0017CE54 File Offset: 0x0017B054
		public Displace(ModuleBase input, ModuleBase x, ModuleBase y, ModuleBase z) : base(4)
		{
			this.modules[0] = input;
			this.modules[1] = x;
			this.modules[2] = y;
			this.modules[3] = z;
		}

		// Token: 0x17000BB8 RID: 3000
		// (get) Token: 0x06003C1A RID: 15386 RVA: 0x0017CE82 File Offset: 0x0017B082
		// (set) Token: 0x06003C1B RID: 15387 RVA: 0x0017CE8C File Offset: 0x0017B08C
		public ModuleBase X
		{
			get
			{
				return this.modules[1];
			}
			set
			{
				this.modules[1] = value;
			}
		}

		// Token: 0x17000BB9 RID: 3001
		// (get) Token: 0x06003C1C RID: 15388 RVA: 0x0017CE97 File Offset: 0x0017B097
		// (set) Token: 0x06003C1D RID: 15389 RVA: 0x0017CEA1 File Offset: 0x0017B0A1
		public ModuleBase Y
		{
			get
			{
				return this.modules[2];
			}
			set
			{
				this.modules[2] = value;
			}
		}

		// Token: 0x17000BBA RID: 3002
		// (get) Token: 0x06003C1E RID: 15390 RVA: 0x0017CEAC File Offset: 0x0017B0AC
		// (set) Token: 0x06003C1F RID: 15391 RVA: 0x0017CEB6 File Offset: 0x0017B0B6
		public ModuleBase Z
		{
			get
			{
				return this.modules[3];
			}
			set
			{
				this.modules[3] = value;
			}
		}

		// Token: 0x06003C20 RID: 15392 RVA: 0x0017CEC4 File Offset: 0x0017B0C4
		public override double GetValue(double x, double y, double z)
		{
			double x2 = x + this.modules[1].GetValue(x, y, z);
			double y2 = y + this.modules[2].GetValue(x, y, z);
			double z2 = z + this.modules[3].GetValue(x, y, z);
			return this.modules[0].GetValue(x2, y2, z2);
		}
	}
}

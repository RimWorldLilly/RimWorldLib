using System;

namespace Verse
{
	// Token: 0x0200016F RID: 367
	public struct GenStepWithParams : IEquatable<GenStepWithParams>
	{
		// Token: 0x06000AE8 RID: 2792 RVA: 0x0003BDE8 File Offset: 0x00039FE8
		public GenStepWithParams(GenStepDef def, GenStepParams parms)
		{
			this.def = def;
			this.parms = parms;
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x0003BDF8 File Offset: 0x00039FF8
		public bool Equals(GenStepWithParams other)
		{
			return object.Equals(this.def, other.def);
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x0003BE0C File Offset: 0x0003A00C
		public override bool Equals(object obj)
		{
			if (obj is GenStepWithParams)
			{
				GenStepWithParams other = (GenStepWithParams)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x0003BE31 File Offset: 0x0003A031
		public override int GetHashCode()
		{
			return HashCode.Combine<GenStepDef, GenStepParams>(this.def, this.parms);
		}

		// Token: 0x04000867 RID: 2151
		public GenStepDef def;

		// Token: 0x04000868 RID: 2152
		public GenStepParams parms;
	}
}

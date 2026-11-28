using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002FB RID: 763
	public struct EdgeSpan
	{
		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x0600174E RID: 5966 RVA: 0x0008C493 File Offset: 0x0008A693
		public bool IsValid
		{
			get
			{
				return this.length > 0;
			}
		}

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x0600174F RID: 5967 RVA: 0x0008C49E File Offset: 0x0008A69E
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				EdgeSpan.<get_Cells>d__6 <get_Cells>d__ = new EdgeSpan.<get_Cells>d__6(-2);
				<get_Cells>d__.<>3__<>4__this = this;
				return <get_Cells>d__;
			}
		}

		// Token: 0x06001750 RID: 5968 RVA: 0x0008C4B4 File Offset: 0x0008A6B4
		public override string ToString()
		{
			string[] array = new string[7];
			array[0] = "(root=";
			int num = 1;
			IntVec3 intVec = this.root;
			array[num] = intVec.ToString();
			array[2] = ", dir=";
			array[3] = this.dir.ToString();
			array[4] = " + length=";
			array[5] = this.length.ToString();
			array[6] = ")";
			return string.Concat(array);
		}

		// Token: 0x06001751 RID: 5969 RVA: 0x0008C525 File Offset: 0x0008A725
		public EdgeSpan(IntVec3 root, SpanDirection dir, int length)
		{
			this.root = root;
			this.dir = dir;
			this.length = length;
		}

		// Token: 0x06001752 RID: 5970 RVA: 0x0008C53C File Offset: 0x0008A73C
		public ulong UniqueHashCode()
		{
			ulong num = this.root.UniqueHashCode();
			if (this.dir == SpanDirection.East)
			{
				num += 17592186044416UL;
			}
			return num + (ulong)(281474976710656L * (long)this.length);
		}

		// Token: 0x040013D8 RID: 5080
		public IntVec3 root;

		// Token: 0x040013D9 RID: 5081
		public SpanDirection dir;

		// Token: 0x040013DA RID: 5082
		public int length;
	}
}

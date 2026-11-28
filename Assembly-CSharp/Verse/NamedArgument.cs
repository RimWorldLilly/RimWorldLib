using System;
using System.Text;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000208 RID: 520
	public struct NamedArgument
	{
		// Token: 0x06000F13 RID: 3859 RVA: 0x000526A1 File Offset: 0x000508A1
		public NamedArgument(object arg, string label)
		{
			this.arg = arg;
			this.label = label;
		}

		// Token: 0x06000F14 RID: 3860 RVA: 0x000526B1 File Offset: 0x000508B1
		public static implicit operator NamedArgument(int value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x000526BF File Offset: 0x000508BF
		public static implicit operator NamedArgument(char value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000F16 RID: 3862 RVA: 0x000526CD File Offset: 0x000508CD
		public static implicit operator NamedArgument(float value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000F17 RID: 3863 RVA: 0x000526DB File Offset: 0x000508DB
		public static implicit operator NamedArgument(double value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000F18 RID: 3864 RVA: 0x000526E9 File Offset: 0x000508E9
		public static implicit operator NamedArgument(long value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000F19 RID: 3865 RVA: 0x000526F7 File Offset: 0x000508F7
		public static implicit operator NamedArgument(string value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000F1A RID: 3866 RVA: 0x00052700 File Offset: 0x00050900
		public static implicit operator NamedArgument(uint value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000F1B RID: 3867 RVA: 0x0005270E File Offset: 0x0005090E
		public static implicit operator NamedArgument(byte value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000F1C RID: 3868 RVA: 0x0005271C File Offset: 0x0005091C
		public static implicit operator NamedArgument(ulong value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x0005272A File Offset: 0x0005092A
		public static implicit operator NamedArgument(StringBuilder value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x00052733 File Offset: 0x00050933
		public static implicit operator NamedArgument(Thing value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x0005273C File Offset: 0x0005093C
		public static implicit operator NamedArgument(Def value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000F20 RID: 3872 RVA: 0x00052745 File Offset: 0x00050945
		public static implicit operator NamedArgument(WorldObject value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000F21 RID: 3873 RVA: 0x0005274E File Offset: 0x0005094E
		public static implicit operator NamedArgument(Faction value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000F22 RID: 3874 RVA: 0x00052757 File Offset: 0x00050957
		public static implicit operator NamedArgument(IntVec3 value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000F23 RID: 3875 RVA: 0x00052765 File Offset: 0x00050965
		public static implicit operator NamedArgument(LocalTargetInfo value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000F24 RID: 3876 RVA: 0x00052773 File Offset: 0x00050973
		public static implicit operator NamedArgument(TargetInfo value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000F25 RID: 3877 RVA: 0x00052781 File Offset: 0x00050981
		public static implicit operator NamedArgument(GlobalTargetInfo value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000F26 RID: 3878 RVA: 0x0005278F File Offset: 0x0005098F
		public static implicit operator NamedArgument(Map value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000F27 RID: 3879 RVA: 0x00052798 File Offset: 0x00050998
		public static implicit operator NamedArgument(TaggedString value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000F28 RID: 3880 RVA: 0x000527A6 File Offset: 0x000509A6
		public static implicit operator NamedArgument(Ideo value)
		{
			return new NamedArgument(value, null);
		}

		// Token: 0x06000F29 RID: 3881 RVA: 0x000527AF File Offset: 0x000509AF
		public override string ToString()
		{
			return (this.label.NullOrEmpty() ? "unnamed" : this.label) + "->" + this.arg.ToStringSafe<object>();
		}

		// Token: 0x04000EA1 RID: 3745
		public object arg;

		// Token: 0x04000EA2 RID: 3746
		public string label;
	}
}

using System;

namespace Verse
{
	// Token: 0x020002A5 RID: 677
	[Flags]
	public enum MeshParts : byte
	{
		// Token: 0x0400124D RID: 4685
		None = 0,
		// Token: 0x0400124E RID: 4686
		Verts = 1,
		// Token: 0x0400124F RID: 4687
		Tris = 2,
		// Token: 0x04001250 RID: 4688
		Colors = 4,
		// Token: 0x04001251 RID: 4689
		UVs1 = 8,
		// Token: 0x04001252 RID: 4690
		UVs2 = 16,
		// Token: 0x04001253 RID: 4691
		Normals = 32,
		// Token: 0x04001254 RID: 4692
		All = 63
	}
}

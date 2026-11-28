using System;

namespace Verse
{
	// Token: 0x0200035C RID: 860
	[Flags]
	public enum CellConnection : byte
	{
		// Token: 0x04001577 RID: 5495
		Self = 0,
		// Token: 0x04001578 RID: 5496
		North = 1,
		// Token: 0x04001579 RID: 5497
		South = 2,
		// Token: 0x0400157A RID: 5498
		East = 4,
		// Token: 0x0400157B RID: 5499
		West = 8,
		// Token: 0x0400157C RID: 5500
		NorthEast = 16,
		// Token: 0x0400157D RID: 5501
		SouthEast = 32,
		// Token: 0x0400157E RID: 5502
		SouthWest = 64,
		// Token: 0x0400157F RID: 5503
		NorthWest = 128,
		// Token: 0x04001580 RID: 5504
		CardinalNeighbors = 15,
		// Token: 0x04001581 RID: 5505
		DiagonalNeighbors = 240,
		// Token: 0x04001582 RID: 5506
		NorthNeighbours = 145,
		// Token: 0x04001583 RID: 5507
		EastNeighbours = 52,
		// Token: 0x04001584 RID: 5508
		SouthNeighbours = 98,
		// Token: 0x04001585 RID: 5509
		WestNeighbours = 200,
		// Token: 0x04001586 RID: 5510
		AllNeighbours = 255
	}
}

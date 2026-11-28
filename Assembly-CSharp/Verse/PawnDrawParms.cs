using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200038C RID: 908
	public struct PawnDrawParms
	{
		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x06001B9C RID: 7068 RVA: 0x000AA642 File Offset: 0x000A8842
		public bool DrawNow
		{
			get
			{
				return this.flags.FlagSet(PawnRenderFlags.DrawNow);
			}
		}

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x06001B9D RID: 7069 RVA: 0x000AA650 File Offset: 0x000A8850
		public bool Portrait
		{
			get
			{
				return this.flags.FlagSet(PawnRenderFlags.Portrait);
			}
		}

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x06001B9E RID: 7070 RVA: 0x000AA65E File Offset: 0x000A885E
		public bool Cache
		{
			get
			{
				return this.flags.FlagSet(PawnRenderFlags.Cache);
			}
		}

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06001B9F RID: 7071 RVA: 0x000AA66D File Offset: 0x000A886D
		public bool Statue
		{
			get
			{
				return this.flags.FlagSet(PawnRenderFlags.Statue);
			}
		}

		// Token: 0x06001BA0 RID: 7072 RVA: 0x000AA680 File Offset: 0x000A8880
		public static PawnDrawParms DefaultFor(Pawn pawn)
		{
			PawnDrawParms @default = PawnDrawParms.Default;
			@default.pawn = pawn;
			@default.facing = pawn.Rotation;
			@default.dead = pawn.Dead;
			@default.crawling = pawn.Crawling;
			@default.swimming = pawn.Swimming;
			return @default;
		}

		// Token: 0x06001BA1 RID: 7073 RVA: 0x000AA6D0 File Offset: 0x000A88D0
		public bool ShouldRecache(PawnDrawParms other)
		{
			return this.pawn == null || this.facing != other.facing || this.rotDrawMode != other.rotDrawMode || this.posture != other.posture || this.flags != other.flags || this.bed != other.bed || this.skipFlags != other.skipFlags || this.flipHead != other.flipHead || this.coveredInFoam != other.coveredInFoam || this.carriedThing != other.carriedThing || this.dead != other.dead || this.crawling != other.crawling || this.swimming != other.swimming || this.statueColor != other.statueColor;
		}

		// Token: 0x040016AA RID: 5802
		public Pawn pawn;

		// Token: 0x040016AB RID: 5803
		public Matrix4x4 matrix;

		// Token: 0x040016AC RID: 5804
		public Rot4 facing;

		// Token: 0x040016AD RID: 5805
		public RotDrawMode rotDrawMode;

		// Token: 0x040016AE RID: 5806
		public PawnPosture posture;

		// Token: 0x040016AF RID: 5807
		public PawnRenderFlags flags;

		// Token: 0x040016B0 RID: 5808
		public Color tint;

		// Token: 0x040016B1 RID: 5809
		public Building_Bed bed;

		// Token: 0x040016B2 RID: 5810
		public ulong skipFlags;

		// Token: 0x040016B3 RID: 5811
		public bool flipHead;

		// Token: 0x040016B4 RID: 5812
		public bool coveredInFoam;

		// Token: 0x040016B5 RID: 5813
		public bool dead;

		// Token: 0x040016B6 RID: 5814
		public bool crawling;

		// Token: 0x040016B7 RID: 5815
		public bool swimming;

		// Token: 0x040016B8 RID: 5816
		public Thing carriedThing;

		// Token: 0x040016B9 RID: 5817
		public Color? statueColor;

		// Token: 0x040016BA RID: 5818
		public static readonly PawnDrawParms Default = new PawnDrawParms
		{
			facing = Rot4.South,
			rotDrawMode = RotDrawMode.Fresh,
			posture = PawnPosture.Standing,
			flags = (PawnRenderFlags.Headgear | PawnRenderFlags.Clothes),
			tint = Color.white
		};
	}
}

using System;

namespace RimWorld
{
	// Token: 0x02001EA0 RID: 7840
	[DefOf]
	public static class PawnRelationDefOf
	{
		// Token: 0x0600CA5B RID: 51803 RVA: 0x004A5ECD File Offset: 0x004A40CD
		static PawnRelationDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(PawnRelationDefOf));
		}

		// Token: 0x04007D1F RID: 32031
		public static PawnRelationDef Kin;

		// Token: 0x04007D20 RID: 32032
		public static PawnRelationDef UncleOrAunt;

		// Token: 0x04007D21 RID: 32033
		public static PawnRelationDef Child;

		// Token: 0x04007D22 RID: 32034
		public static PawnRelationDef Grandchild;

		// Token: 0x04007D23 RID: 32035
		public static PawnRelationDef Grandparent;

		// Token: 0x04007D24 RID: 32036
		public static PawnRelationDef Sibling;

		// Token: 0x04007D25 RID: 32037
		public static PawnRelationDef HalfSibling;

		// Token: 0x04007D26 RID: 32038
		public static PawnRelationDef Cousin;

		// Token: 0x04007D27 RID: 32039
		public static PawnRelationDef NephewOrNiece;

		// Token: 0x04007D28 RID: 32040
		public static PawnRelationDef Spouse;

		// Token: 0x04007D29 RID: 32041
		public static PawnRelationDef Parent;

		// Token: 0x04007D2A RID: 32042
		public static PawnRelationDef GreatGrandchild;

		// Token: 0x04007D2B RID: 32043
		public static PawnRelationDef GreatGrandparent;

		// Token: 0x04007D2C RID: 32044
		public static PawnRelationDef GranduncleOrGrandaunt;

		// Token: 0x04007D2D RID: 32045
		public static PawnRelationDef Lover;

		// Token: 0x04007D2E RID: 32046
		public static PawnRelationDef ExLover;

		// Token: 0x04007D2F RID: 32047
		public static PawnRelationDef ExSpouse;

		// Token: 0x04007D30 RID: 32048
		public static PawnRelationDef Fiance;

		// Token: 0x04007D31 RID: 32049
		public static PawnRelationDef Bond;

		// Token: 0x04007D32 RID: 32050
		[MayRequireBiotech]
		public static PawnRelationDef Overseer;

		// Token: 0x04007D33 RID: 32051
		[MayRequireBiotech]
		public static PawnRelationDef ParentBirth;
	}
}

using System;
using System.Collections.Generic;
using LudeonTK;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x020020A8 RID: 8360
	public static class GrowthUtility
	{
		// Token: 0x0600D2C4 RID: 53956 RVA: 0x004D85A0 File Offset: 0x004D67A0
		public static bool IsGrowthBirthday(int age)
		{
			for (int i = 0; i < GrowthUtility.GrowthMomentAges.Length; i++)
			{
				if (age == GrowthUtility.GrowthMomentAges[i])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600D2C5 RID: 53957 RVA: 0x004D85CC File Offset: 0x004D67CC
		[DebugOutput("Text generation", true)]
		private static void GrowthMomentFlavor()
		{
			Pawn targetPawn;
			if (!PawnsFinder.AllMaps_FreeColonists.TryRandomElement(out targetPawn))
			{
				Log.Error("No colonists to generate growth moment flavor for.");
				return;
			}
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			for (int i = 0; i < GrowthUtility.GrowthTiers.Length; i++)
			{
				int tier = i;
				list.Add(new DebugMenuOption("Tier " + tier.ToString(), DebugMenuOptionMode.Action, delegate()
				{
					string text = string.Concat(new string[]
					{
						"Samples for ",
						targetPawn.NameShortColored.Resolve(),
						" at tier ",
						tier.ToString(),
						":"
					});
					for (int j = 0; j < 10; j++)
					{
						text = text + "\n  - " + GrowthUtility.GrowthFlavorForTier(targetPawn, tier);
					}
					Log.Message(text);
				}));
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x0600D2C6 RID: 53958 RVA: 0x004D8668 File Offset: 0x004D6868
		public static string GrowthFlavorForTier(Pawn pawn, int growthTier)
		{
			int num;
			if (growthTier >= 8)
			{
				num = 4;
			}
			else if (growthTier >= 6)
			{
				num = 3;
			}
			else if (growthTier >= 4)
			{
				num = 2;
			}
			else if (growthTier >= 1)
			{
				num = 1;
			}
			else
			{
				num = 0;
			}
			GrammarRequest request = default(GrammarRequest);
			request.Includes.Add(RulePackDefOf.GrowthMomentFlavor);
			request.Constants.Add("tierSection", num.ToString());
			request.Rules.AddRange(GrammarUtility.RulesForPawn("PAWN", pawn, request.Constants, false, true));
			return GrammarResolver.Resolve("r_root", request, null, false, null, null, null, true);
		}

		// Token: 0x040085A7 RID: 34215
		public static readonly GrowthUtility.GrowthTier[] GrowthTiers = new GrowthUtility.GrowthTier[]
		{
			new GrowthUtility.GrowthTier(0, 0f, ByteRange.Zero, 0, 1, 1),
			new GrowthUtility.GrowthTier(1, 30f, ByteRange.Zero, 0, 1, 2),
			new GrowthUtility.GrowthTier(2, 55f, ByteRange.Zero, 0, 1, 3),
			new GrowthUtility.GrowthTier(3, 80f, ByteRange.Zero, 0, 1, 4),
			new GrowthUtility.GrowthTier(4, 100f, ByteRange.One, 1, 1, 4),
			new GrowthUtility.GrowthTier(5, 120f, ByteRange.One, 2, 1, 4),
			new GrowthUtility.GrowthTier(6, 135f, ByteRange.One, 3, 1, 4),
			new GrowthUtility.GrowthTier(7, 150f, new ByteRange(2, 2), 4, 1, 4),
			new GrowthUtility.GrowthTier(8, 162f, new ByteRange(3, 4), 6, 1, 6)
		};

		// Token: 0x040085A8 RID: 34216
		public static readonly int[] GrowthMomentAges = new int[]
		{
			7,
			10,
			13
		};

		// Token: 0x02003C72 RID: 15474
		public readonly struct GrowthTier
		{
			// Token: 0x06015A3D RID: 88637 RVA: 0x006CF20E File Offset: 0x006CD40E
			public GrowthTier(byte tier, float pointsRequirement, ByteRange passionGainsRange, byte passionChoices, byte traitGains, byte traitChoices)
			{
				this.tier = tier;
				this.pointsRequirement = pointsRequirement;
				this.passionGainsRange = passionGainsRange;
				this.passionChoices = passionChoices;
				this.traitGains = traitGains;
				this.traitChoices = traitChoices;
			}

			// Token: 0x06015A3E RID: 88638 RVA: 0x006CF240 File Offset: 0x006CD440
			public int PassionGainFor(Pawn pawn)
			{
				int randomInRange;
				using (new RandBlock(Gen.HashCombineInt(pawn.GetHashCode(), (int)this.tier)))
				{
					randomInRange = this.passionGainsRange.RandomInRange;
				}
				return randomInRange;
			}

			// Token: 0x0400EF96 RID: 61334
			public readonly byte tier;

			// Token: 0x0400EF97 RID: 61335
			public readonly float pointsRequirement;

			// Token: 0x0400EF98 RID: 61336
			public readonly ByteRange passionGainsRange;

			// Token: 0x0400EF99 RID: 61337
			public readonly byte passionChoices;

			// Token: 0x0400EF9A RID: 61338
			public readonly byte traitGains;

			// Token: 0x0400EF9B RID: 61339
			public readonly byte traitChoices;
		}
	}
}

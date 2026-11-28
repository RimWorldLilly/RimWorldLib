using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D0E RID: 7438
	public class CreditsAssembler
	{
		// Token: 0x0600C05A RID: 49242 RVA: 0x00462B37 File Offset: 0x00460D37
		public static IEnumerable<CreditsEntry> AllCredits()
		{
			return new CreditsAssembler.<AllCredits>d__0(-2);
		}

		// Token: 0x0600C05C RID: 49244 RVA: 0x00462B48 File Offset: 0x00460D48
		[CompilerGenerated]
		internal static IEnumerable<CreditsEntry> <AllCredits>g__Reformat2Cols|0_0(List<CreditsEntry> entries)
		{
			CreditsAssembler.<<AllCredits>g__Reformat2Cols|0_0>d <<AllCredits>g__Reformat2Cols|0_0>d = new CreditsAssembler.<<AllCredits>g__Reformat2Cols|0_0>d(-2);
			<<AllCredits>g__Reformat2Cols|0_0>d.<>3__entries = entries;
			return <<AllCredits>g__Reformat2Cols|0_0>d;
		}
	}
}

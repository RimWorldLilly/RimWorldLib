using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x020000D2 RID: 210
	public static class BookUtility
	{
		// Token: 0x06000811 RID: 2065 RVA: 0x0002EF6C File Offset: 0x0002D16C
		public static bool CanReadEver(Pawn reader)
		{
			return reader.DevelopmentalStage != DevelopmentalStage.Baby && !StatDefOf.ReadingSpeed.Worker.IsDisabledFor(reader) && reader.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) && reader.health.capacities.CapableOf(PawnCapacityDefOf.Sight);
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x0002EFCB File Offset: 0x0002D1CB
		public static bool CanReadNow(Pawn reader)
		{
			return BookUtility.CanReadEver(reader) && BookUtility.GetReadingModifier(reader) > 0f;
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x0002EFE8 File Offset: 0x0002D1E8
		public static bool CanReadBook(Book book, Pawn reader, out string reason)
		{
			if (!book.IsReadable)
			{
				reason = "BookNotReadable".Translate(book.Named("BOOK"));
				return false;
			}
			DevelopmentalStage developmentalStageFilter = book.BookComp.Props.developmentalStageFilter;
			if (!developmentalStageFilter.HasAny(reader.DevelopmentalStage))
			{
				string arg = developmentalStageFilter.ToCommaList(false);
				reason = "BookCantBeStage".Translate(reader.Named("PAWN"), arg.Named("STAGES"));
				return false;
			}
			if (!reader.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
			{
				reason = "BookBlind".Translate(reader.Named("PAWN"));
				return false;
			}
			if (!BookUtility.CanReadEver(reader))
			{
				reason = "BookCantRead".Translate(reader.Named("PAWN"));
				return false;
			}
			reason = null;
			return true;
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x0002F0CA File Offset: 0x0002D2CA
		public static float GetReadingModifier(Pawn reader)
		{
			if (reader == null || StatDefOf.ReadingSpeed.Worker.IsDisabledFor(reader))
			{
				return 1f;
			}
			return reader.GetStatValue(StatDefOf.ReadingSpeed, true, -1);
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x0002F0F4 File Offset: 0x0002D2F4
		public static Book MakeBook(ArtGenerationContext context, QualityGenerator? qualityGenerator = null)
		{
			return BookUtility.MakeBook(BookUtility.GetBookDefs().RandomElementByWeight((ThingDef x) => x.GetCompProperties<CompProperties_Book>().pickWeight), context, qualityGenerator);
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x0002F128 File Offset: 0x0002D328
		public static Book MakeBook(ThingDef def, ArtGenerationContext context, QualityGenerator? qualityGenerator = null)
		{
			ThingDef stuff = GenStuff.RandomStuffFor(def);
			Thing thing = ThingMaker.MakeThing(def, stuff);
			CompQuality compQuality = thing.TryGetComp<CompQuality>();
			if (compQuality != null)
			{
				QualityCategory q = (qualityGenerator != null) ? QualityUtility.GenerateQuality(qualityGenerator.Value) : QualityUtility.GenerateQualityRandomEqualChance();
				compQuality.SetQuality(q, new ArtGenerationContext?(context));
			}
			return thing as Book;
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x0002F17C File Offset: 0x0002D37C
		private static List<ThingDef> GetBookDefs()
		{
			return (from x in DefDatabase<ThingDef>.AllDefsListForReading
			where x.HasComp<CompBook>()
			select x).ToList<ThingDef>();
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x0002F1AC File Offset: 0x0002D3AC
		public static float GetReadingBonus(Thing thing)
		{
			Room room = thing.GetRoom(RegionType.Set_All);
			if (room != null && room.ProperRoom && !room.PsychologicallyOutdoors)
			{
				return room.GetStat(RoomStatDefOf.ReadingBonus);
			}
			return 1f;
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x0002F1E8 File Offset: 0x0002D3E8
		public static bool TryGetRandomBookToRead(Pawn pawn, out Book book)
		{
			book = null;
			BookUtility.TmpCandidates.Clear();
			BookUtility.TmpOutcomeCandidates.Clear();
			BookUtility.TmpCandidates.AddRange(from thing in pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.Book)
			where BookUtility.IsValidBook(thing, pawn)
			select thing);
			BookUtility.TmpCandidates.AddRange(from thing in pawn.Map.listerThings.GetThingsOfType<Building_Bookcase>().SelectMany((Building_Bookcase x) => x.HeldBooks)
			where BookUtility.IsValidBook(thing, pawn)
			select thing);
			if (BookUtility.TmpCandidates.Empty<Thing>())
			{
				return false;
			}
			foreach (Thing thing2 in BookUtility.TmpCandidates)
			{
				Book book2 = thing2 as Book;
				if (book2 != null && book2.ProvidesOutcome(pawn))
				{
					BookUtility.TmpOutcomeCandidates.Add(thing2);
				}
			}
			book = (Book)(BookUtility.TmpOutcomeCandidates.Any<Thing>() ? BookUtility.TmpOutcomeCandidates.RandomElement<Thing>() : BookUtility.TmpCandidates.RandomElement<Thing>());
			BookUtility.TmpCandidates.Clear();
			BookUtility.TmpOutcomeCandidates.Clear();
			return true;
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x0002F34C File Offset: 0x0002D54C
		private static bool IsValidBook(Thing thing, Pawn pawn)
		{
			if (thing is Book && !thing.IsForbiddenHeld(pawn))
			{
				Pawn_ReadingTracker reading = pawn.reading;
				if (((reading != null) ? reading.CurrentPolicy : null) != null && pawn.reading.CurrentPolicy.defFilter.Allows(thing) && pawn.reading.CurrentPolicy.effectFilter.Allows(thing) && pawn.CanReserveAndReach(thing, PathEndMode.Touch, Danger.None, 1, -1, null, false) && !thing.Fogged() && thing.IsPoliticallyProper(pawn))
				{
					return !thing.VacuumConcernTo(pawn);
				}
			}
			return false;
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x0002F3DF File Offset: 0x0002D5DF
		public static float GetResearchExpForQuality(QualityCategory quality)
		{
			return BookUtility.QualityResearchExpTick.Evaluate((float)quality);
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x0002F3ED File Offset: 0x0002D5ED
		public static float GetAnomalyExpForQuality(QualityCategory quality)
		{
			return BookUtility.QualityAnomalyExpTick.Evaluate((float)quality);
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x0002F3FB File Offset: 0x0002D5FB
		public static float GetSkillExpForQuality(QualityCategory quality)
		{
			return BookUtility.QualitySkillExpTick.Evaluate((float)quality);
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x0002F409 File Offset: 0x0002D609
		public static float GetNovelJoyFactorForQuality(QualityCategory quality)
		{
			return BookUtility.QualityJoyFactor.Evaluate((float)quality);
		}

		// Token: 0x040003A9 RID: 937
		private static readonly List<Thing> TmpCandidates = new List<Thing>();

		// Token: 0x040003AA RID: 938
		private static readonly List<Thing> TmpOutcomeCandidates = new List<Thing>();

		// Token: 0x040003AB RID: 939
		private static readonly SimpleCurve QualityResearchExpTick = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.008f),
				true
			},
			{
				new CurvePoint(1f, 0.012f),
				true
			},
			{
				new CurvePoint(2f, 0.016f),
				true
			},
			{
				new CurvePoint(3f, 0.02f),
				true
			},
			{
				new CurvePoint(4f, 0.024f),
				true
			},
			{
				new CurvePoint(5f, 0.028f),
				true
			},
			{
				new CurvePoint(6f, 0.032f),
				true
			}
		};

		// Token: 0x040003AC RID: 940
		private static readonly SimpleCurve QualityAnomalyExpTick = new SimpleCurve
		{
			{
				new CurvePoint(0f, 3E-05f),
				true
			},
			{
				new CurvePoint(1f, 6E-05f),
				true
			},
			{
				new CurvePoint(2f, 9E-05f),
				true
			},
			{
				new CurvePoint(3f, 0.00012f),
				true
			},
			{
				new CurvePoint(4f, 0.00015f),
				true
			},
			{
				new CurvePoint(5f, 0.00018f),
				true
			},
			{
				new CurvePoint(6f, 0.00021f),
				true
			}
		};

		// Token: 0x040003AD RID: 941
		private static readonly SimpleCurve QualitySkillExpTick = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.05f),
				true
			},
			{
				new CurvePoint(1f, 0.075f),
				true
			},
			{
				new CurvePoint(2f, 0.1f),
				true
			},
			{
				new CurvePoint(3f, 0.125f),
				true
			},
			{
				new CurvePoint(4f, 0.15f),
				true
			},
			{
				new CurvePoint(5f, 0.175f),
				true
			},
			{
				new CurvePoint(6f, 0.2f),
				true
			}
		};

		// Token: 0x040003AE RID: 942
		private static readonly SimpleCurve QualityJoyFactor = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1.2f),
				true
			},
			{
				new CurvePoint(1f, 1.4f),
				true
			},
			{
				new CurvePoint(2f, 1.6f),
				true
			},
			{
				new CurvePoint(3f, 1.8f),
				true
			},
			{
				new CurvePoint(4f, 2f),
				true
			},
			{
				new CurvePoint(5f, 2.25f),
				true
			},
			{
				new CurvePoint(6f, 2.5f),
				true
			}
		};
	}
}

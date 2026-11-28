using System;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse
{
	// Token: 0x020000F0 RID: 240
	public struct TraverseParms : IEquatable<TraverseParms>
	{
		// Token: 0x0600089E RID: 2206 RVA: 0x00030D5C File Offset: 0x0002EF5C
		public static TraverseParms For(Pawn pawn, Danger maxDanger = Danger.Deadly, TraverseMode mode = TraverseMode.ByPawn, bool canBashDoors = false, bool alwaysUseAvoidGrid = false, bool canBashFences = false, bool avoidPersistentDanger = true)
		{
			if (pawn == null)
			{
				Log.Error("TraverseParms for null pawn.");
				return TraverseParms.For(TraverseMode.NoPassClosedDoors, maxDanger, canBashDoors, alwaysUseAvoidGrid, canBashFences, true, false);
			}
			TraverseParms result = new TraverseParms
			{
				pawn = pawn,
				maxDanger = maxDanger,
				mode = mode,
				canBashDoors = canBashDoors,
				canBashFences = canBashFences,
				alwaysUseAvoidGrid = alwaysUseAvoidGrid,
				fenceBlocked = pawn.ShouldAvoidFences,
				avoidPersistentDanger = avoidPersistentDanger,
				avoidDarknessDanger = GameCondition_UnnaturalDarkness.AffectedByDarkness(pawn),
				avoidFog = (pawn.IsPlayerControlled && !pawn.Drafted)
			};
			if (pawn.CurJob != null)
			{
				IBuildableDriver buildableDriver = pawn.CurJob.GetCachedDriver(pawn) as IBuildableDriver;
				CellRect cellRect;
				if (buildableDriver != null && buildableDriver.TryGetBuildableRect(out cellRect))
				{
					result.targetBuildable = cellRect;
				}
			}
			return result;
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x00030E30 File Offset: 0x0002F030
		public static TraverseParms For(TraverseMode mode, Danger maxDanger = Danger.Deadly, bool canBashDoors = false, bool alwaysUseAvoidGrid = false, bool canBashFences = false, bool avoidPersistentDanger = true, bool fogBlocked = false)
		{
			return new TraverseParms
			{
				pawn = null,
				mode = mode,
				maxDanger = maxDanger,
				canBashDoors = canBashDoors,
				canBashFences = canBashFences,
				alwaysUseAvoidGrid = alwaysUseAvoidGrid,
				fenceBlocked = false,
				avoidPersistentDanger = avoidPersistentDanger,
				avoidFog = fogBlocked
			};
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x00030E91 File Offset: 0x0002F091
		public TraverseParms WithFenceblockedOf(Pawn otherPawn)
		{
			return this.WithFenceblocked(otherPawn.ShouldAvoidFences);
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x00030EA0 File Offset: 0x0002F0A0
		public TraverseParms WithFenceblocked(bool forceFenceblocked)
		{
			return new TraverseParms
			{
				pawn = this.pawn,
				mode = this.mode,
				maxDanger = this.maxDanger,
				canBashDoors = this.canBashDoors,
				canBashFences = this.canBashFences,
				alwaysUseAvoidGrid = this.alwaysUseAvoidGrid,
				fenceBlocked = (this.fenceBlocked || forceFenceblocked),
				avoidPersistentDanger = true
			};
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x00030F1B File Offset: 0x0002F11B
		public void Validate()
		{
			if (this.mode == TraverseMode.ByPawn && this.pawn == null)
			{
				Log.Error("Invalid traverse parameters: IfPawnAllowed but traverser = null.");
			}
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x00030F37 File Offset: 0x0002F137
		public static implicit operator TraverseParms(TraverseMode m)
		{
			if (m == TraverseMode.ByPawn)
			{
				throw new InvalidOperationException("Cannot implicitly convert TraverseMode.ByPawn to RegionTraverseParameters.");
			}
			return TraverseParms.For(m, Danger.Deadly, false, false, false, true, false);
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x00030F54 File Offset: 0x0002F154
		public static bool operator ==(TraverseParms a, TraverseParms b)
		{
			return a.pawn == b.pawn && a.mode == b.mode && a.canBashDoors == b.canBashDoors && a.canBashFences == b.canBashFences && a.maxDanger == b.maxDanger && a.alwaysUseAvoidGrid == b.alwaysUseAvoidGrid && a.fenceBlocked == b.fenceBlocked && a.avoidPersistentDanger == b.avoidPersistentDanger;
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x00030FD3 File Offset: 0x0002F1D3
		public static bool operator !=(TraverseParms a, TraverseParms b)
		{
			return !(a == b);
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x00030FE0 File Offset: 0x0002F1E0
		public override bool Equals(object obj)
		{
			if (obj is TraverseParms)
			{
				TraverseParms other = (TraverseParms)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x00031008 File Offset: 0x0002F208
		public bool Equals(TraverseParms other)
		{
			return other.pawn == this.pawn && other.mode == this.mode && other.canBashDoors == this.canBashDoors && other.canBashFences == this.canBashFences && other.maxDanger == this.maxDanger && other.alwaysUseAvoidGrid == this.alwaysUseAvoidGrid && other.fenceBlocked == this.fenceBlocked && other.avoidPersistentDanger == this.avoidPersistentDanger;
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x00031088 File Offset: 0x0002F288
		public override int GetHashCode()
		{
			int seed = (this.canBashDoors > false) ? 1 : 0;
			seed = ((this.pawn != null) ? Gen.HashCombine<Pawn>(seed, this.pawn) : Gen.HashCombineStruct<TraverseMode>(seed, this.mode));
			seed = Gen.HashCombineStruct<bool>(seed, this.canBashFences);
			seed = Gen.HashCombineStruct<Danger>(seed, this.maxDanger);
			seed = Gen.HashCombineStruct<bool>(seed, this.alwaysUseAvoidGrid);
			seed = Gen.HashCombineStruct<bool>(seed, this.fenceBlocked);
			return Gen.HashCombineStruct<bool>(seed, this.avoidPersistentDanger);
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x00031104 File Offset: 0x0002F304
		public override string ToString()
		{
			string value = this.canBashDoors ? " canBashDoors" : "";
			string value2 = this.canBashFences ? " canBashFences" : "";
			string value3 = this.alwaysUseAvoidGrid ? " alwaysUseAvoidGrid" : "";
			string value4 = this.fenceBlocked ? " fenceBlocked" : "";
			string value5 = this.avoidPersistentDanger ? " avoidPersistentDanger" : "";
			if (this.mode == TraverseMode.ByPawn)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 8);
				defaultInterpolatedStringHandler.AppendLiteral("(");
				defaultInterpolatedStringHandler.AppendFormatted<TraverseMode>(this.mode);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted<Danger>(this.maxDanger);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted<Pawn>(this.pawn);
				defaultInterpolatedStringHandler.AppendFormatted(value);
				defaultInterpolatedStringHandler.AppendFormatted(value2);
				defaultInterpolatedStringHandler.AppendFormatted(value3);
				defaultInterpolatedStringHandler.AppendFormatted(value4);
				defaultInterpolatedStringHandler.AppendFormatted(value5);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(3, 7);
			defaultInterpolatedStringHandler2.AppendLiteral("(");
			defaultInterpolatedStringHandler2.AppendFormatted<TraverseMode>(this.mode);
			defaultInterpolatedStringHandler2.AppendLiteral(" ");
			defaultInterpolatedStringHandler2.AppendFormatted<Danger>(this.maxDanger);
			defaultInterpolatedStringHandler2.AppendFormatted(value);
			defaultInterpolatedStringHandler2.AppendFormatted(value2);
			defaultInterpolatedStringHandler2.AppendFormatted(value3);
			defaultInterpolatedStringHandler2.AppendFormatted(value4);
			defaultInterpolatedStringHandler2.AppendFormatted(value5);
			defaultInterpolatedStringHandler2.AppendLiteral(")");
			return defaultInterpolatedStringHandler2.ToStringAndClear();
		}

		// Token: 0x040003DC RID: 988
		public Pawn pawn;

		// Token: 0x040003DD RID: 989
		public TraverseMode mode;

		// Token: 0x040003DE RID: 990
		public Danger maxDanger;

		// Token: 0x040003DF RID: 991
		public bool canBashDoors;

		// Token: 0x040003E0 RID: 992
		public bool canBashFences;

		// Token: 0x040003E1 RID: 993
		public bool alwaysUseAvoidGrid;

		// Token: 0x040003E2 RID: 994
		public bool fenceBlocked;

		// Token: 0x040003E3 RID: 995
		public bool avoidPersistentDanger;

		// Token: 0x040003E4 RID: 996
		public bool avoidDarknessDanger;

		// Token: 0x040003E5 RID: 997
		public bool avoidFog;

		// Token: 0x040003E6 RID: 998
		public CellRect targetBuildable;
	}
}

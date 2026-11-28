using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020004D9 RID: 1241
	public struct PawnGenerationRequest
	{
		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x060023D2 RID: 9170 RVA: 0x000DAA82 File Offset: 0x000D8C82
		// (set) Token: 0x060023D3 RID: 9171 RVA: 0x000DAAA4 File Offset: 0x000D8CA4
		public PawnKindDef KindDef
		{
			get
			{
				if (this.PawnKindDefGetter != null)
				{
					return this.PawnKindDefGetter(this.ForcedXenotype);
				}
				return this.kindDefInner;
			}
			set
			{
				this.kindDefInner = value;
			}
		}

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x060023D4 RID: 9172 RVA: 0x000DAAAD File Offset: 0x000D8CAD
		// (set) Token: 0x060023D5 RID: 9173 RVA: 0x000DAAB5 File Offset: 0x000D8CB5
		public PawnGenerationContext Context { readonly get; set; }

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x060023D6 RID: 9174 RVA: 0x000DAABE File Offset: 0x000D8CBE
		// (set) Token: 0x060023D7 RID: 9175 RVA: 0x000DAAC6 File Offset: 0x000D8CC6
		public Faction Faction { readonly get; set; }

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x060023D8 RID: 9176 RVA: 0x000DAACF File Offset: 0x000D8CCF
		// (set) Token: 0x060023D9 RID: 9177 RVA: 0x000DAAD7 File Offset: 0x000D8CD7
		public PlanetTile Tile { readonly get; set; }

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x060023DA RID: 9178 RVA: 0x000DAAE0 File Offset: 0x000D8CE0
		// (set) Token: 0x060023DB RID: 9179 RVA: 0x000DAAE8 File Offset: 0x000D8CE8
		public bool ForceGenerateNewPawn { readonly get; set; }

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x060023DC RID: 9180 RVA: 0x000DAAF1 File Offset: 0x000D8CF1
		// (set) Token: 0x060023DD RID: 9181 RVA: 0x000DAAF9 File Offset: 0x000D8CF9
		public BodyTypeDef ForceBodyType { readonly get; set; }

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x060023DE RID: 9182 RVA: 0x000DAB02 File Offset: 0x000D8D02
		// (set) Token: 0x060023DF RID: 9183 RVA: 0x000DAB0A File Offset: 0x000D8D0A
		public bool AllowDead { readonly get; set; }

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x060023E0 RID: 9184 RVA: 0x000DAB13 File Offset: 0x000D8D13
		// (set) Token: 0x060023E1 RID: 9185 RVA: 0x000DAB1B File Offset: 0x000D8D1B
		public bool AllowDowned { readonly get; set; }

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x060023E2 RID: 9186 RVA: 0x000DAB24 File Offset: 0x000D8D24
		// (set) Token: 0x060023E3 RID: 9187 RVA: 0x000DAB2C File Offset: 0x000D8D2C
		public bool CanGeneratePawnRelations { readonly get; set; }

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x060023E4 RID: 9188 RVA: 0x000DAB35 File Offset: 0x000D8D35
		// (set) Token: 0x060023E5 RID: 9189 RVA: 0x000DAB3D File Offset: 0x000D8D3D
		public bool MustBeCapableOfViolence { readonly get; set; }

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x060023E6 RID: 9190 RVA: 0x000DAB46 File Offset: 0x000D8D46
		// (set) Token: 0x060023E7 RID: 9191 RVA: 0x000DAB4E File Offset: 0x000D8D4E
		public float ColonistRelationChanceFactor { readonly get; set; }

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x060023E8 RID: 9192 RVA: 0x000DAB57 File Offset: 0x000D8D57
		// (set) Token: 0x060023E9 RID: 9193 RVA: 0x000DAB5F File Offset: 0x000D8D5F
		public bool ForceAddFreeWarmLayerIfNeeded { readonly get; set; }

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x060023EA RID: 9194 RVA: 0x000DAB68 File Offset: 0x000D8D68
		// (set) Token: 0x060023EB RID: 9195 RVA: 0x000DAB70 File Offset: 0x000D8D70
		public bool AllowGay { readonly get; set; }

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x060023EC RID: 9196 RVA: 0x000DAB79 File Offset: 0x000D8D79
		// (set) Token: 0x060023ED RID: 9197 RVA: 0x000DAB81 File Offset: 0x000D8D81
		public bool AllowPregnant { readonly get; set; }

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x060023EE RID: 9198 RVA: 0x000DAB8A File Offset: 0x000D8D8A
		// (set) Token: 0x060023EF RID: 9199 RVA: 0x000DAB92 File Offset: 0x000D8D92
		public bool AllowFood { readonly get; set; }

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x060023F0 RID: 9200 RVA: 0x000DAB9B File Offset: 0x000D8D9B
		// (set) Token: 0x060023F1 RID: 9201 RVA: 0x000DABA3 File Offset: 0x000D8DA3
		public bool AllowAddictions { readonly get; set; }

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x060023F2 RID: 9202 RVA: 0x000DABAC File Offset: 0x000D8DAC
		// (set) Token: 0x060023F3 RID: 9203 RVA: 0x000DABB4 File Offset: 0x000D8DB4
		public IEnumerable<TraitDef> ForcedTraits { readonly get; set; }

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x060023F4 RID: 9204 RVA: 0x000DABBD File Offset: 0x000D8DBD
		// (set) Token: 0x060023F5 RID: 9205 RVA: 0x000DABC5 File Offset: 0x000D8DC5
		public IEnumerable<TraitDef> ProhibitedTraits { readonly get; set; }

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x060023F6 RID: 9206 RVA: 0x000DABCE File Offset: 0x000D8DCE
		// (set) Token: 0x060023F7 RID: 9207 RVA: 0x000DABD6 File Offset: 0x000D8DD6
		public bool Inhabitant { readonly get; set; }

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x060023F8 RID: 9208 RVA: 0x000DABDF File Offset: 0x000D8DDF
		// (set) Token: 0x060023F9 RID: 9209 RVA: 0x000DABE7 File Offset: 0x000D8DE7
		public bool CertainlyBeenInCryptosleep { readonly get; set; }

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x060023FA RID: 9210 RVA: 0x000DABF0 File Offset: 0x000D8DF0
		// (set) Token: 0x060023FB RID: 9211 RVA: 0x000DABF8 File Offset: 0x000D8DF8
		public bool ForceRedressWorldPawnIfFormerColonist { readonly get; set; }

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x060023FC RID: 9212 RVA: 0x000DAC01 File Offset: 0x000D8E01
		// (set) Token: 0x060023FD RID: 9213 RVA: 0x000DAC09 File Offset: 0x000D8E09
		public bool WorldPawnFactionDoesntMatter { readonly get; set; }

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x060023FE RID: 9214 RVA: 0x000DAC12 File Offset: 0x000D8E12
		// (set) Token: 0x060023FF RID: 9215 RVA: 0x000DAC1A File Offset: 0x000D8E1A
		public float BiocodeWeaponChance { readonly get; set; }

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x06002400 RID: 9216 RVA: 0x000DAC23 File Offset: 0x000D8E23
		// (set) Token: 0x06002401 RID: 9217 RVA: 0x000DAC2B File Offset: 0x000D8E2B
		public float BiocodeApparelChance { readonly get; set; }

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x06002402 RID: 9218 RVA: 0x000DAC34 File Offset: 0x000D8E34
		// (set) Token: 0x06002403 RID: 9219 RVA: 0x000DAC3C File Offset: 0x000D8E3C
		public Pawn ExtraPawnForExtraRelationChance { readonly get; set; }

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x06002404 RID: 9220 RVA: 0x000DAC45 File Offset: 0x000D8E45
		// (set) Token: 0x06002405 RID: 9221 RVA: 0x000DAC4D File Offset: 0x000D8E4D
		public float RelationWithExtraPawnChanceFactor { readonly get; set; }

		// Token: 0x17000755 RID: 1877
		// (get) Token: 0x06002406 RID: 9222 RVA: 0x000DAC56 File Offset: 0x000D8E56
		// (set) Token: 0x06002407 RID: 9223 RVA: 0x000DAC5E File Offset: 0x000D8E5E
		public Predicate<Pawn> RedressValidator { readonly get; set; }

		// Token: 0x17000756 RID: 1878
		// (get) Token: 0x06002408 RID: 9224 RVA: 0x000DAC67 File Offset: 0x000D8E67
		// (set) Token: 0x06002409 RID: 9225 RVA: 0x000DAC6F File Offset: 0x000D8E6F
		public bool DontGiveWeapon { readonly get; set; }

		// Token: 0x17000757 RID: 1879
		// (get) Token: 0x0600240A RID: 9226 RVA: 0x000DAC78 File Offset: 0x000D8E78
		// (set) Token: 0x0600240B RID: 9227 RVA: 0x000DAC80 File Offset: 0x000D8E80
		public bool OnlyUseForcedBackstories { readonly get; set; }

		// Token: 0x17000758 RID: 1880
		// (get) Token: 0x0600240C RID: 9228 RVA: 0x000DAC89 File Offset: 0x000D8E89
		// (set) Token: 0x0600240D RID: 9229 RVA: 0x000DAC91 File Offset: 0x000D8E91
		public Predicate<Pawn> ValidatorPreGear { readonly get; set; }

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x0600240E RID: 9230 RVA: 0x000DAC9A File Offset: 0x000D8E9A
		// (set) Token: 0x0600240F RID: 9231 RVA: 0x000DACA2 File Offset: 0x000D8EA2
		public Predicate<Pawn> ValidatorPostGear { readonly get; set; }

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x06002410 RID: 9232 RVA: 0x000DACAB File Offset: 0x000D8EAB
		// (set) Token: 0x06002411 RID: 9233 RVA: 0x000DACB3 File Offset: 0x000D8EB3
		public float? MinChanceToRedressWorldPawn { readonly get; set; }

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x06002412 RID: 9234 RVA: 0x000DACBC File Offset: 0x000D8EBC
		// (set) Token: 0x06002413 RID: 9235 RVA: 0x000DACC4 File Offset: 0x000D8EC4
		public float? FixedBiologicalAge { readonly get; set; }

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06002414 RID: 9236 RVA: 0x000DACCD File Offset: 0x000D8ECD
		// (set) Token: 0x06002415 RID: 9237 RVA: 0x000DACD5 File Offset: 0x000D8ED5
		public float? FixedChronologicalAge { readonly get; set; }

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x06002416 RID: 9238 RVA: 0x000DACDE File Offset: 0x000D8EDE
		// (set) Token: 0x06002417 RID: 9239 RVA: 0x000DACE6 File Offset: 0x000D8EE6
		public Gender? FixedGender { readonly get; set; }

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06002418 RID: 9240 RVA: 0x000DACEF File Offset: 0x000D8EEF
		// (set) Token: 0x06002419 RID: 9241 RVA: 0x000DACF7 File Offset: 0x000D8EF7
		public string FixedLastName { readonly get; set; }

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x0600241A RID: 9242 RVA: 0x000DAD00 File Offset: 0x000D8F00
		// (set) Token: 0x0600241B RID: 9243 RVA: 0x000DAD08 File Offset: 0x000D8F08
		public string FixedBirthName { readonly get; set; }

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x0600241C RID: 9244 RVA: 0x000DAD11 File Offset: 0x000D8F11
		// (set) Token: 0x0600241D RID: 9245 RVA: 0x000DAD19 File Offset: 0x000D8F19
		public RoyalTitleDef FixedTitle { readonly get; set; }

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x0600241E RID: 9246 RVA: 0x000DAD22 File Offset: 0x000D8F22
		// (set) Token: 0x0600241F RID: 9247 RVA: 0x000DAD2A File Offset: 0x000D8F2A
		public bool ForbidAnyTitle { readonly get; set; }

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x06002420 RID: 9248 RVA: 0x000DAD33 File Offset: 0x000D8F33
		// (set) Token: 0x06002421 RID: 9249 RVA: 0x000DAD3B File Offset: 0x000D8F3B
		public Ideo FixedIdeo { readonly get; set; }

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x06002422 RID: 9250 RVA: 0x000DAD44 File Offset: 0x000D8F44
		// (set) Token: 0x06002423 RID: 9251 RVA: 0x000DAD4C File Offset: 0x000D8F4C
		public bool ForceNoIdeo { readonly get; set; }

		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x06002424 RID: 9252 RVA: 0x000DAD55 File Offset: 0x000D8F55
		// (set) Token: 0x06002425 RID: 9253 RVA: 0x000DAD5D File Offset: 0x000D8F5D
		public bool ForceNoBackstory { readonly get; set; }

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x06002426 RID: 9254 RVA: 0x000DAD66 File Offset: 0x000D8F66
		// (set) Token: 0x06002427 RID: 9255 RVA: 0x000DAD6E File Offset: 0x000D8F6E
		public bool ForceDead { readonly get; set; }

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x06002428 RID: 9256 RVA: 0x000DAD77 File Offset: 0x000D8F77
		// (set) Token: 0x06002429 RID: 9257 RVA: 0x000DAD7F File Offset: 0x000D8F7F
		public List<GeneDef> ForcedXenogenes { readonly get; set; }

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x0600242A RID: 9258 RVA: 0x000DAD88 File Offset: 0x000D8F88
		// (set) Token: 0x0600242B RID: 9259 RVA: 0x000DAD90 File Offset: 0x000D8F90
		public List<GeneDef> ForcedEndogenes { readonly get; set; }

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x0600242C RID: 9260 RVA: 0x000DAD99 File Offset: 0x000D8F99
		// (set) Token: 0x0600242D RID: 9261 RVA: 0x000DADA1 File Offset: 0x000D8FA1
		public XenotypeDef ForcedXenotype { readonly get; set; }

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x0600242E RID: 9262 RVA: 0x000DADAA File Offset: 0x000D8FAA
		// (set) Token: 0x0600242F RID: 9263 RVA: 0x000DADB2 File Offset: 0x000D8FB2
		public List<XenotypeDef> AllowedXenotypes { readonly get; set; }

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x06002430 RID: 9264 RVA: 0x000DADBB File Offset: 0x000D8FBB
		// (set) Token: 0x06002431 RID: 9265 RVA: 0x000DADC3 File Offset: 0x000D8FC3
		public float ForceBaselinerChance { readonly get; set; }

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x06002432 RID: 9266 RVA: 0x000DADCC File Offset: 0x000D8FCC
		// (set) Token: 0x06002433 RID: 9267 RVA: 0x000DADD4 File Offset: 0x000D8FD4
		public CustomXenotype ForcedCustomXenotype { readonly get; set; }

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x06002434 RID: 9268 RVA: 0x000DADDD File Offset: 0x000D8FDD
		// (set) Token: 0x06002435 RID: 9269 RVA: 0x000DADE5 File Offset: 0x000D8FE5
		public DevelopmentalStage AllowedDevelopmentalStages { readonly get; set; }

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x06002436 RID: 9270 RVA: 0x000DADEE File Offset: 0x000D8FEE
		// (set) Token: 0x06002437 RID: 9271 RVA: 0x000DADF6 File Offset: 0x000D8FF6
		public Func<XenotypeDef, PawnKindDef> PawnKindDefGetter { readonly get; set; }

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x06002438 RID: 9272 RVA: 0x000DADFF File Offset: 0x000D8FFF
		// (set) Token: 0x06002439 RID: 9273 RVA: 0x000DAE07 File Offset: 0x000D9007
		public FloatRange? ExcludeBiologicalAgeRange { readonly get; set; }

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x0600243A RID: 9274 RVA: 0x000DAE10 File Offset: 0x000D9010
		// (set) Token: 0x0600243B RID: 9275 RVA: 0x000DAE18 File Offset: 0x000D9018
		public FloatRange? BiologicalAgeRange { readonly get; set; }

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x0600243C RID: 9276 RVA: 0x000DAE21 File Offset: 0x000D9021
		// (set) Token: 0x0600243D RID: 9277 RVA: 0x000DAE29 File Offset: 0x000D9029
		public bool ForceRecruitable { readonly get; set; }

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x0600243E RID: 9278 RVA: 0x000DAE32 File Offset: 0x000D9032
		// (set) Token: 0x0600243F RID: 9279 RVA: 0x000DAE3A File Offset: 0x000D903A
		public int MinimumAgeTraits { readonly get; set; }

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x06002440 RID: 9280 RVA: 0x000DAE43 File Offset: 0x000D9043
		// (set) Token: 0x06002441 RID: 9281 RVA: 0x000DAE4B File Offset: 0x000D904B
		public int MaximumAgeTraits { readonly get; set; }

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x06002442 RID: 9282 RVA: 0x000DAE54 File Offset: 0x000D9054
		// (set) Token: 0x06002443 RID: 9283 RVA: 0x000DAE5C File Offset: 0x000D905C
		public bool ForceNoGear { readonly get; set; }

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x06002444 RID: 9284 RVA: 0x000DAE65 File Offset: 0x000D9065
		// (set) Token: 0x06002445 RID: 9285 RVA: 0x000DAE6D File Offset: 0x000D906D
		public MutantDef ForcedMutant { readonly get; set; }

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x06002446 RID: 9286 RVA: 0x000DAE76 File Offset: 0x000D9076
		// (set) Token: 0x06002447 RID: 9287 RVA: 0x000DAE7E File Offset: 0x000D907E
		public bool IsCreepJoiner { readonly get; set; }

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x06002448 RID: 9288 RVA: 0x000DAE87 File Offset: 0x000D9087
		// (set) Token: 0x06002449 RID: 9289 RVA: 0x000DAE8F File Offset: 0x000D908F
		public bool DontGivePreArrivalPathway { readonly get; set; }

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x0600244A RID: 9290 RVA: 0x000DAE98 File Offset: 0x000D9098
		// (set) Token: 0x0600244B RID: 9291 RVA: 0x000DAEA0 File Offset: 0x000D90A0
		public bool ForceNoIdeoGear { readonly get; set; }

		// Token: 0x0600244C RID: 9292 RVA: 0x000DAEAC File Offset: 0x000D90AC
		public PawnGenerationRequest(PawnKindDef kind, Faction faction = null, PawnGenerationContext context = PawnGenerationContext.NonPlayer, PlanetTile? tile = null, bool forceGenerateNewPawn = false, bool allowDead = false, bool allowDowned = false, bool canGeneratePawnRelations = true, bool mustBeCapableOfViolence = false, float colonistRelationChanceFactor = 1f, bool forceAddFreeWarmLayerIfNeeded = false, bool allowGay = true, bool allowPregnant = false, bool allowFood = true, bool allowAddictions = true, bool inhabitant = false, bool certainlyBeenInCryptosleep = false, bool forceRedressWorldPawnIfFormerColonist = false, bool worldPawnFactionDoesntMatter = false, float biocodeWeaponChance = 0f, float biocodeApparelChance = 0f, Pawn extraPawnForExtraRelationChance = null, float relationWithExtraPawnChanceFactor = 1f, Predicate<Pawn> validatorPreGear = null, Predicate<Pawn> validatorPostGear = null, IEnumerable<TraitDef> forcedTraits = null, IEnumerable<TraitDef> prohibitedTraits = null, float? minChanceToRedressWorldPawn = null, float? fixedBiologicalAge = null, float? fixedChronologicalAge = null, Gender? fixedGender = null, string fixedLastName = null, string fixedBirthName = null, RoyalTitleDef fixedTitle = null, Ideo fixedIdeo = null, bool forceNoIdeo = false, bool forceNoBackstory = false, bool forbidAnyTitle = false, bool forceDead = false, List<GeneDef> forcedXenogenes = null, List<GeneDef> forcedEndogenes = null, XenotypeDef forcedXenotype = null, CustomXenotype forcedCustomXenotype = null, List<XenotypeDef> allowedXenotypes = null, float forceBaselinerChance = 0f, DevelopmentalStage developmentalStages = DevelopmentalStage.Adult, Func<XenotypeDef, PawnKindDef> pawnKindDefGetter = null, FloatRange? excludeBiologicalAgeRange = null, FloatRange? biologicalAgeRange = null, bool forceRecruitable = false, bool dontGiveWeapon = false, bool onlyUseForcedBackstories = false, int maximumAgeTraits = -1, int minimumAgeTraits = 0, bool forceNoGear = false)
		{
			this = default(PawnGenerationRequest);
			this._calledTheCorrectConstructor = true;
			this.KindDef = kind;
			this.Context = context;
			this.Tile = (tile ?? PlanetTile.Invalid);
			this.Faction = faction;
			this.ForceGenerateNewPawn = forceGenerateNewPawn;
			this.AllowDead = allowDead;
			this.AllowDowned = allowDowned;
			this.CanGeneratePawnRelations = canGeneratePawnRelations;
			this.MustBeCapableOfViolence = mustBeCapableOfViolence;
			this.ColonistRelationChanceFactor = colonistRelationChanceFactor;
			this.ForceAddFreeWarmLayerIfNeeded = forceAddFreeWarmLayerIfNeeded;
			this.AllowGay = allowGay;
			this.AllowPregnant = allowPregnant;
			this.AllowFood = allowFood;
			this.AllowAddictions = allowAddictions;
			this.ForcedTraits = forcedTraits;
			this.ProhibitedTraits = prohibitedTraits;
			this.Inhabitant = inhabitant;
			this.CertainlyBeenInCryptosleep = certainlyBeenInCryptosleep;
			this.ForceRedressWorldPawnIfFormerColonist = forceRedressWorldPawnIfFormerColonist;
			this.WorldPawnFactionDoesntMatter = worldPawnFactionDoesntMatter;
			this.ExtraPawnForExtraRelationChance = extraPawnForExtraRelationChance;
			this.RelationWithExtraPawnChanceFactor = relationWithExtraPawnChanceFactor;
			this.BiocodeWeaponChance = biocodeWeaponChance;
			this.BiocodeApparelChance = biocodeApparelChance;
			this.ForceNoIdeo = forceNoIdeo;
			this.ForceNoBackstory = forceNoBackstory;
			this.ForbidAnyTitle = forbidAnyTitle;
			this.ValidatorPreGear = validatorPreGear;
			this.ValidatorPostGear = validatorPostGear;
			this.MinChanceToRedressWorldPawn = minChanceToRedressWorldPawn;
			this.FixedBiologicalAge = fixedBiologicalAge;
			this.FixedChronologicalAge = fixedChronologicalAge;
			this.FixedGender = fixedGender;
			this.FixedLastName = fixedLastName;
			this.FixedBirthName = fixedBirthName;
			this.FixedTitle = fixedTitle;
			this.FixedIdeo = fixedIdeo;
			this.ForceDead = forceDead;
			this.ForcedXenotype = forcedXenotype;
			this.ForcedCustomXenotype = forcedCustomXenotype;
			this.AllowedXenotypes = allowedXenotypes;
			this.ForceBaselinerChance = forceBaselinerChance;
			this.AllowedDevelopmentalStages = developmentalStages;
			this.PawnKindDefGetter = pawnKindDefGetter;
			this.ExcludeBiologicalAgeRange = excludeBiologicalAgeRange;
			this.BiologicalAgeRange = biologicalAgeRange;
			this.ForceRecruitable = forceRecruitable;
			this.DontGiveWeapon = dontGiveWeapon;
			this.OnlyUseForcedBackstories = onlyUseForcedBackstories;
			this.MaximumAgeTraits = maximumAgeTraits;
			this.MinimumAgeTraits = minimumAgeTraits;
			this.ForceNoGear = forceNoGear;
			if (forcedXenogenes != null)
			{
				foreach (GeneDef gene in forcedXenogenes)
				{
					this.AddForcedGene(gene, true);
				}
			}
			if (forcedEndogenes != null)
			{
				foreach (GeneDef gene2 in forcedEndogenes)
				{
					this.AddForcedGene(gene2, false);
				}
			}
			this.ValidateAndFix();
		}

		// Token: 0x0600244D RID: 9293 RVA: 0x000DB114 File Offset: 0x000D9314
		public void ValidateAndFix()
		{
			if (!this._calledTheCorrectConstructor)
			{
				Log.Error("This PawnGenerationRequest was not created through the correct constructor.");
				this._calledTheCorrectConstructor = true;
			}
			if (this.Context == PawnGenerationContext.All)
			{
				Log.Error("Should not generate pawns with context 'All'");
				this.Context = PawnGenerationContext.NonPlayer;
			}
			if (this.ForceNoIdeo && this.FixedIdeo != null)
			{
				Log.Error("Trying to generate a pawn with no ideo and a fixed ideo.");
				this.ForceNoIdeo = false;
			}
			if ((this.AllowedDevelopmentalStages.Newborn() || this.AllowedDevelopmentalStages.Baby()) && this.FixedIdeo != null)
			{
				Log.Error("Trying to generate baby with specific ideology (babies have no ideology).");
				this.FixedIdeo = null;
			}
			string value;
			if (!this.AllowDowned && this.AlwaysDownedLifeStages(out value))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(87, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Trying to generate a non-downed ");
				defaultInterpolatedStringHandler.AppendFormatted<DevelopmentalStage>(this.AllowedDevelopmentalStages);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted(this.KindDef.label);
				defaultInterpolatedStringHandler.AppendLiteral(" pawn but that would include these downed lifestages: ");
				defaultInterpolatedStringHandler.AppendFormatted(value);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				this.AllowDowned = true;
			}
			if (this.AllowedDevelopmentalStages.Newborn() && this.AllowedDevelopmentalStages != DevelopmentalStage.Newborn)
			{
				Log.Error("Trying to generate a newborn and other developmental stages simultaneously.");
				this.AllowedDevelopmentalStages = DevelopmentalStage.Newborn;
			}
			if (this.ExcludeBiologicalAgeRange != null && this.FixedBiologicalAge != null)
			{
				Log.Error("Trying to generate a pawn with a fixed biological age and an excluded biological age range.");
				this.ExcludeBiologicalAgeRange = null;
			}
			if (this.BiologicalAgeRange != null && this.FixedBiologicalAge != null)
			{
				Log.Error("Trying to generate a pawn with a fixed biological age and a biological age range.");
				this.BiologicalAgeRange = null;
			}
			if (this.BiologicalAgeRange != null && this.ExcludeBiologicalAgeRange != null)
			{
				Log.Error("Trying to generate a pawn with both a include and exclude age range");
				this.BiologicalAgeRange = null;
				this.ExcludeBiologicalAgeRange = null;
			}
		}

		// Token: 0x0600244E RID: 9294 RVA: 0x000DB304 File Offset: 0x000D9504
		[Obsolete("Will be removed in 1.5; use the constructor instead.")]
		public static PawnGenerationRequest MakeDefault()
		{
			return new PawnGenerationRequest(null, null, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false);
		}

		// Token: 0x0600244F RID: 9295 RVA: 0x000DB399 File Offset: 0x000D9599
		public void SetFixedLastName(string fixedLastName)
		{
			if (this.FixedLastName != null)
			{
				Log.Error("Last name is already a fixed value: " + this.FixedLastName + ".");
				return;
			}
			this.FixedLastName = fixedLastName;
		}

		// Token: 0x06002450 RID: 9296 RVA: 0x000DB3C5 File Offset: 0x000D95C5
		public void SetFixedBirthName(string fixedBirthName)
		{
			if (this.FixedBirthName != null)
			{
				Log.Error("birth name is already a fixed value: " + this.FixedBirthName + ".");
				return;
			}
			this.FixedBirthName = fixedBirthName;
		}

		// Token: 0x06002451 RID: 9297 RVA: 0x000DB3F4 File Offset: 0x000D95F4
		public void AddForcedGene(GeneDef gene, bool xenogene)
		{
			if (xenogene)
			{
				if (this.ForcedXenogenes == null)
				{
					this.ForcedXenogenes = new List<GeneDef>();
				}
				this.ForcedXenogenes.Add(gene);
				return;
			}
			if (this.ForcedEndogenes == null)
			{
				this.ForcedEndogenes = new List<GeneDef>();
			}
			this.ForcedEndogenes.Add(gene);
		}

		// Token: 0x06002452 RID: 9298 RVA: 0x000DB444 File Offset: 0x000D9644
		public override string ToString()
		{
			string[] array = new string[52];
			array[0] = "kindDef=";
			int num = 1;
			PawnKindDef kindDef = this.KindDef;
			array[num] = ((kindDef != null) ? kindDef.ToString() : null);
			array[2] = ", context=";
			array[3] = this.Context.ToString();
			array[4] = ", faction=";
			int num2 = 5;
			Faction faction = this.Faction;
			array[num2] = ((faction != null) ? faction.ToString() : null);
			array[6] = ", tile=";
			array[7] = this.Tile.ToString();
			array[8] = ", forceGenerateNewPawn=";
			array[9] = this.ForceGenerateNewPawn.ToString();
			array[10] = ", allowedDevelopmentalStages=";
			array[11] = this.AllowedDevelopmentalStages.ToString();
			array[12] = ", allowDead=";
			array[13] = this.AllowDead.ToString();
			array[14] = ", allowDowned=";
			array[15] = this.AllowDowned.ToString();
			array[16] = ", canGeneratePawnRelations=";
			array[17] = this.CanGeneratePawnRelations.ToString();
			array[18] = ", mustBeCapableOfViolence=";
			array[19] = this.MustBeCapableOfViolence.ToString();
			array[20] = ", colonistRelationChanceFactor=";
			array[21] = this.ColonistRelationChanceFactor.ToString();
			array[22] = ", forceAddFreeWarmLayerIfNeeded=";
			array[23] = this.ForceAddFreeWarmLayerIfNeeded.ToString();
			array[24] = ", allowGay=";
			array[25] = this.AllowGay.ToString();
			array[26] = ", prohibitedTraits=";
			int num3 = 27;
			IEnumerable<TraitDef> prohibitedTraits = this.ProhibitedTraits;
			array[num3] = ((prohibitedTraits != null) ? prohibitedTraits.ToString() : null);
			array[28] = ", allowFood=";
			array[29] = this.AllowFood.ToString();
			array[30] = ", allowAddictions=";
			array[31] = this.AllowAddictions.ToString();
			array[32] = ", inhabitant=";
			array[33] = this.Inhabitant.ToString();
			array[34] = ", certainlyBeenInCryptosleep=";
			array[35] = this.CertainlyBeenInCryptosleep.ToString();
			array[36] = ", biocodeWeaponChance=";
			array[37] = this.BiocodeWeaponChance.ToString();
			array[38] = ", validatorPreGear=";
			int num4 = 39;
			Predicate<Pawn> validatorPreGear = this.ValidatorPreGear;
			array[num4] = ((validatorPreGear != null) ? validatorPreGear.ToString() : null);
			array[40] = ", validatorPostGear=";
			int num5 = 41;
			Predicate<Pawn> validatorPostGear = this.ValidatorPostGear;
			array[num5] = ((validatorPostGear != null) ? validatorPostGear.ToString() : null);
			array[42] = ", fixedBiologicalAge=";
			array[43] = this.FixedBiologicalAge.ToString();
			array[44] = ", fixedChronologicalAge=";
			array[45] = this.FixedChronologicalAge.ToString();
			array[46] = ", fixedGender=";
			array[47] = this.FixedGender.ToString();
			array[48] = ", fixedLastName=";
			array[49] = this.FixedLastName;
			array[50] = ", fixedBirthName=";
			array[51] = this.FixedBirthName;
			return string.Concat(array);
		}

		// Token: 0x06002453 RID: 9299 RVA: 0x000DB740 File Offset: 0x000D9940
		private bool AlwaysDownedLifeStages(out string stages)
		{
			DevelopmentalStage developmentalStage = this.AllowedDevelopmentalStages;
			if (this.AllowedDevelopmentalStages.Newborn())
			{
				developmentalStage |= DevelopmentalStage.Baby;
			}
			StringBuilder stringBuilder = null;
			foreach (LifeStageAge lifeStageAge in this.KindDef.RaceProps.lifeStageAges)
			{
				if (lifeStageAge.def.alwaysDowned && developmentalStage.Has(lifeStageAge.def.developmentalStage))
				{
					if (stringBuilder == null)
					{
						stringBuilder = new StringBuilder(lifeStageAge.def.label, 64);
					}
					else
					{
						stringBuilder.Append(", ");
						stringBuilder.Append(lifeStageAge.def.label);
					}
				}
			}
			if (stringBuilder == null)
			{
				stages = null;
				return false;
			}
			stages = stringBuilder.ToString();
			return true;
		}

		// Token: 0x04001A50 RID: 6736
		private PawnKindDef kindDefInner;

		// Token: 0x04001A8D RID: 6797
		private bool _calledTheCorrectConstructor;
	}
}

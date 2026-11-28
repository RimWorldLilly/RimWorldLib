using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C2D RID: 7213
	[StaticConstructorOnStartup]
	public static class PortraitsCache
	{
		// Token: 0x0600B800 RID: 47104 RVA: 0x0040F190 File Offset: 0x0040D390
		public static RenderTexture Get(Pawn pawn, Vector2 size, Rot4 rotation, Vector3 cameraOffset = default(Vector3), float cameraZoom = 1f, bool supersample = true, bool compensateForUIScale = true, bool renderHeadgear = true, bool renderClothes = true, IReadOnlyDictionary<Apparel, Color> overrideApparelColors = null, Color? overrideHairColor = null, bool stylingStation = false, PawnHealthState? healthStateOverride = null)
		{
			if (supersample)
			{
				size *= 1.25f;
			}
			if (compensateForUIScale)
			{
				size *= Prefs.UIScale;
			}
			PortraitsCache.PortraitParams portraitParams = new PortraitsCache.PortraitParams(size, cameraOffset, cameraZoom, rotation, renderHeadgear, renderClothes, overrideApparelColors, overrideHairColor, stylingStation, healthStateOverride);
			Dictionary<Pawn, PortraitsCache.CachedPortrait> orCreateCachedPortraitsWithParams = PortraitsCache.GetOrCreateCachedPortraitsWithParams(portraitParams);
			PortraitsCache.CachedPortrait cachedPortrait;
			if (orCreateCachedPortraitsWithParams.TryGetValue(pawn, out cachedPortrait))
			{
				if (!cachedPortrait.RenderTexture.IsCreated())
				{
					cachedPortrait.RenderTexture.Create();
					portraitParams.RenderPortrait(pawn, cachedPortrait.RenderTexture);
				}
				else if (cachedPortrait.Dirty)
				{
					portraitParams.RenderPortrait(pawn, cachedPortrait.RenderTexture);
				}
				orCreateCachedPortraitsWithParams.Remove(pawn);
				orCreateCachedPortraitsWithParams.Add(pawn, new PortraitsCache.CachedPortrait(cachedPortrait.RenderTexture, false, Time.time));
				return cachedPortrait.RenderTexture;
			}
			RenderTexture renderTexture = PortraitsCache.NewRenderTexture(size);
			portraitParams.RenderPortrait(pawn, renderTexture);
			orCreateCachedPortraitsWithParams.Add(pawn, new PortraitsCache.CachedPortrait(renderTexture, false, Time.time));
			return renderTexture;
		}

		// Token: 0x0600B801 RID: 47105 RVA: 0x0040F27C File Offset: 0x0040D47C
		public static void SetDirty(Pawn pawn)
		{
			foreach (KeyValuePair<PortraitsCache.PortraitParams, Dictionary<Pawn, PortraitsCache.CachedPortrait>> keyValuePair in PortraitsCache.cachedPortraits)
			{
				PortraitsCache.PortraitParams portraitParams;
				Dictionary<Pawn, PortraitsCache.CachedPortrait> dictionary;
				keyValuePair.Deconstruct(out portraitParams, out dictionary);
				Dictionary<Pawn, PortraitsCache.CachedPortrait> dictionary2 = dictionary;
				PortraitsCache.CachedPortrait cachedPortrait;
				if (dictionary2.TryGetValue(pawn, out cachedPortrait) && !cachedPortrait.Dirty)
				{
					dictionary2.Remove(pawn);
					dictionary2.Add(pawn, new PortraitsCache.CachedPortrait(cachedPortrait.RenderTexture, true, cachedPortrait.LastUseTime));
				}
			}
		}

		// Token: 0x0600B802 RID: 47106 RVA: 0x0040F30C File Offset: 0x0040D50C
		public static void PortraitsCacheUpdate()
		{
			PortraitsCache.RemoveExpiredCachedPortraits();
			PortraitsCache.SetAnimatedPortraitsDirty();
		}

		// Token: 0x0600B803 RID: 47107 RVA: 0x0040F318 File Offset: 0x0040D518
		public static void Clear()
		{
			foreach (KeyValuePair<PortraitsCache.PortraitParams, Dictionary<Pawn, PortraitsCache.CachedPortrait>> keyValuePair in PortraitsCache.cachedPortraits)
			{
				PortraitsCache.PortraitParams portraitParams;
				Dictionary<Pawn, PortraitsCache.CachedPortrait> dictionary;
				keyValuePair.Deconstruct(out portraitParams, out dictionary);
				foreach (KeyValuePair<Pawn, PortraitsCache.CachedPortrait> keyValuePair2 in dictionary)
				{
					PortraitsCache.DestroyRenderTexture(keyValuePair2.Value.RenderTexture);
				}
			}
			PortraitsCache.cachedPortraits.Clear();
			for (int i = 0; i < PortraitsCache.renderTexturesPool.Count; i++)
			{
				PortraitsCache.DestroyRenderTexture(PortraitsCache.renderTexturesPool[i]);
			}
			PortraitsCache.renderTexturesPool.Clear();
		}

		// Token: 0x0600B804 RID: 47108 RVA: 0x0040F3FC File Offset: 0x0040D5FC
		private static Dictionary<Pawn, PortraitsCache.CachedPortrait> GetOrCreateCachedPortraitsWithParams(PortraitsCache.PortraitParams portraitParams)
		{
			Dictionary<Pawn, PortraitsCache.CachedPortrait> dictionary;
			if (!PortraitsCache.cachedPortraits.TryGetValue(portraitParams, out dictionary))
			{
				dictionary = new Dictionary<Pawn, PortraitsCache.CachedPortrait>(new PawnEqualityComparer());
				PortraitsCache.cachedPortraits.Add(portraitParams, dictionary);
			}
			return dictionary;
		}

		// Token: 0x0600B805 RID: 47109 RVA: 0x0040F430 File Offset: 0x0040D630
		private static void DestroyRenderTexture(RenderTexture rt)
		{
			Object.Destroy(rt);
		}

		// Token: 0x0600B806 RID: 47110 RVA: 0x0040F438 File Offset: 0x0040D638
		private static void RemoveExpiredCachedPortraits()
		{
			foreach (KeyValuePair<PortraitsCache.PortraitParams, Dictionary<Pawn, PortraitsCache.CachedPortrait>> keyValuePair in PortraitsCache.cachedPortraits)
			{
				PortraitsCache.PortraitParams portraitParams;
				Dictionary<Pawn, PortraitsCache.CachedPortrait> dictionary;
				keyValuePair.Deconstruct(out portraitParams, out dictionary);
				Dictionary<Pawn, PortraitsCache.CachedPortrait> dictionary2 = dictionary;
				PortraitsCache.toRemove.Clear();
				foreach (KeyValuePair<Pawn, PortraitsCache.CachedPortrait> keyValuePair2 in dictionary2)
				{
					if (keyValuePair2.Value.Expired)
					{
						PortraitsCache.toRemove.Add(keyValuePair2.Key);
						PortraitsCache.renderTexturesPool.Add(keyValuePair2.Value.RenderTexture);
					}
				}
				for (int i = 0; i < PortraitsCache.toRemove.Count; i++)
				{
					dictionary2.Remove(PortraitsCache.toRemove[i]);
				}
				PortraitsCache.toRemove.Clear();
			}
		}

		// Token: 0x0600B807 RID: 47111 RVA: 0x0040F54C File Offset: 0x0040D74C
		private static void SetAnimatedPortraitsDirty()
		{
			foreach (KeyValuePair<PortraitsCache.PortraitParams, Dictionary<Pawn, PortraitsCache.CachedPortrait>> keyValuePair in PortraitsCache.cachedPortraits)
			{
				PortraitsCache.PortraitParams portraitParams;
				Dictionary<Pawn, PortraitsCache.CachedPortrait> dictionary;
				keyValuePair.Deconstruct(out portraitParams, out dictionary);
				Dictionary<Pawn, PortraitsCache.CachedPortrait> dictionary2 = dictionary;
				PortraitsCache.toSetDirty.Clear();
				foreach (KeyValuePair<Pawn, PortraitsCache.CachedPortrait> keyValuePair2 in dictionary2)
				{
					if (PortraitsCache.IsAnimated(keyValuePair2.Key) && !keyValuePair2.Value.Dirty)
					{
						PortraitsCache.toSetDirty.Add(keyValuePair2.Key);
					}
				}
				for (int i = 0; i < PortraitsCache.toSetDirty.Count; i++)
				{
					PortraitsCache.CachedPortrait cachedPortrait = dictionary2[PortraitsCache.toSetDirty[i]];
					dictionary2.Remove(PortraitsCache.toSetDirty[i]);
					dictionary2.Add(PortraitsCache.toSetDirty[i], new PortraitsCache.CachedPortrait(cachedPortrait.RenderTexture, true, cachedPortrait.LastUseTime));
				}
				PortraitsCache.toSetDirty.Clear();
			}
		}

		// Token: 0x0600B808 RID: 47112 RVA: 0x0040F6A8 File Offset: 0x0040D8A8
		private static RenderTexture NewRenderTexture(Vector2 size)
		{
			int num = PortraitsCache.renderTexturesPool.FindLastIndex((RenderTexture x) => x.width == (int)size.x && x.height == (int)size.y);
			if (num != -1)
			{
				RenderTexture result = PortraitsCache.renderTexturesPool[num];
				PortraitsCache.renderTexturesPool.RemoveAt(num);
				return result;
			}
			return new RenderTexture((int)size.x, (int)size.y, 24)
			{
				name = "Portrait",
				useMipMap = false,
				filterMode = FilterMode.Bilinear
			};
		}

		// Token: 0x0600B809 RID: 47113 RVA: 0x0040F72C File Offset: 0x0040D92C
		private static bool IsAnimated(Pawn pawn)
		{
			return Current.ProgramState == ProgramState.Playing && pawn.Drawer.renderer.flasher.FlashingNowOrRecently;
		}

		// Token: 0x04006996 RID: 27030
		private static List<RenderTexture> renderTexturesPool = new List<RenderTexture>();

		// Token: 0x04006997 RID: 27031
		private static Dictionary<PortraitsCache.PortraitParams, Dictionary<Pawn, PortraitsCache.CachedPortrait>> cachedPortraits = new Dictionary<PortraitsCache.PortraitParams, Dictionary<Pawn, PortraitsCache.CachedPortrait>>(new PortraitsCache.PortraitParamsEqualityComparer());

		// Token: 0x04006998 RID: 27032
		private const float SupersampleScale = 1.25f;

		// Token: 0x04006999 RID: 27033
		private static List<Pawn> toRemove = new List<Pawn>();

		// Token: 0x0400699A RID: 27034
		private static List<Pawn> toSetDirty = new List<Pawn>();

		// Token: 0x020038D2 RID: 14546
		private struct CachedPortrait
		{
			// Token: 0x170035AF RID: 13743
			// (get) Token: 0x06014B61 RID: 84833 RVA: 0x006A0EA1 File Offset: 0x0069F0A1
			// (set) Token: 0x06014B62 RID: 84834 RVA: 0x006A0EA9 File Offset: 0x0069F0A9
			public RenderTexture RenderTexture { readonly get; private set; }

			// Token: 0x170035B0 RID: 13744
			// (get) Token: 0x06014B63 RID: 84835 RVA: 0x006A0EB2 File Offset: 0x0069F0B2
			// (set) Token: 0x06014B64 RID: 84836 RVA: 0x006A0EBA File Offset: 0x0069F0BA
			public bool Dirty { readonly get; private set; }

			// Token: 0x170035B1 RID: 13745
			// (get) Token: 0x06014B65 RID: 84837 RVA: 0x006A0EC3 File Offset: 0x0069F0C3
			// (set) Token: 0x06014B66 RID: 84838 RVA: 0x006A0ECB File Offset: 0x0069F0CB
			public float LastUseTime { readonly get; private set; }

			// Token: 0x170035B2 RID: 13746
			// (get) Token: 0x06014B67 RID: 84839 RVA: 0x006A0ED4 File Offset: 0x0069F0D4
			public bool Expired
			{
				get
				{
					return Time.time - this.LastUseTime > 1f;
				}
			}

			// Token: 0x06014B68 RID: 84840 RVA: 0x006A0EE9 File Offset: 0x0069F0E9
			public CachedPortrait(RenderTexture renderTexture, bool dirty, float lastUseTime)
			{
				this = default(PortraitsCache.CachedPortrait);
				this.RenderTexture = renderTexture;
				this.Dirty = dirty;
				this.LastUseTime = lastUseTime;
			}

			// Token: 0x0400E2E2 RID: 58082
			private const float CacheDuration = 1f;
		}

		// Token: 0x020038D3 RID: 14547
		private class PortraitParamsEqualityComparer : IEqualityComparer<PortraitsCache.PortraitParams>
		{
			// Token: 0x06014B69 RID: 84841 RVA: 0x006A0F07 File Offset: 0x0069F107
			public bool Equals(PortraitsCache.PortraitParams x, PortraitsCache.PortraitParams y)
			{
				return x.Equals(y);
			}

			// Token: 0x06014B6A RID: 84842 RVA: 0x006A0F11 File Offset: 0x0069F111
			public int GetHashCode(PortraitsCache.PortraitParams obj)
			{
				return obj.GetHashCode();
			}
		}

		// Token: 0x020038D4 RID: 14548
		private readonly struct PortraitParams : IEquatable<PortraitsCache.PortraitParams>
		{
			// Token: 0x06014B6C RID: 84844 RVA: 0x006A0F28 File Offset: 0x0069F128
			public PortraitParams(Vector2 size, Vector3 cameraOffset, float cameraZoom, Rot4 rotation, bool renderHeadgear = true, bool renderClothes = true, IReadOnlyDictionary<Apparel, Color> overrideApparelColors = null, Color? overrideHairColor = null, bool stylingStation = false, PawnHealthState? overrideHealthState = null)
			{
				this.size = size;
				this.cameraOffset = cameraOffset;
				this.cameraZoom = cameraZoom;
				this.rotation = rotation;
				this.renderHeadgear = renderHeadgear;
				this.renderClothes = renderClothes;
				this.overrideApparelColors = overrideApparelColors;
				this.overrideHairColor = overrideHairColor;
				this.stylingStation = stylingStation;
				this.overrideHealthState = overrideHealthState;
				this.cachedHashCode = (size.GetHashCode() ^ cameraOffset.GetHashCode() ^ cameraZoom.GetHashCode() ^ rotation.GetHashCode() ^ renderHeadgear.GetHashCode() ^ renderClothes.GetHashCode() ^ stylingStation.GetHashCode() ^ ((overrideHairColor != null) ? overrideHairColor.GetValueOrDefault().GetHashCode() : 0) ^ ((overrideHealthState != null) ? overrideHealthState.GetValueOrDefault().GetHashCode() : 0) ^ GenCollection.DictHashCode<Apparel, Color>(overrideApparelColors));
			}

			// Token: 0x06014B6D RID: 84845 RVA: 0x006A1020 File Offset: 0x0069F220
			public override bool Equals(object obj)
			{
				if (obj is PortraitsCache.PortraitParams)
				{
					PortraitsCache.PortraitParams other = (PortraitsCache.PortraitParams)obj;
					return this.Equals(other);
				}
				return false;
			}

			// Token: 0x06014B6E RID: 84846 RVA: 0x006A1048 File Offset: 0x0069F248
			public bool Equals(PortraitsCache.PortraitParams other)
			{
				if (other.size == this.size && other.cameraOffset == this.cameraOffset && other.cameraZoom == this.cameraZoom && other.rotation == this.rotation && other.renderHeadgear == this.renderHeadgear && other.renderClothes == this.renderClothes && other.stylingStation == this.stylingStation && other.overrideHairColor == this.overrideHairColor)
				{
					PawnHealthState? pawnHealthState = other.overrideHealthState;
					PawnHealthState? pawnHealthState2 = this.overrideHealthState;
					if (pawnHealthState.GetValueOrDefault() == pawnHealthState2.GetValueOrDefault() & pawnHealthState != null == (pawnHealthState2 != null))
					{
						return GenCollection.DictsEqual<Apparel, Color>(other.overrideApparelColors, this.overrideApparelColors);
					}
				}
				return false;
			}

			// Token: 0x06014B6F RID: 84847 RVA: 0x006A1161 File Offset: 0x0069F361
			public override int GetHashCode()
			{
				return this.cachedHashCode;
			}

			// Token: 0x06014B70 RID: 84848 RVA: 0x006A116C File Offset: 0x0069F36C
			public void RenderPortrait(Pawn pawn, RenderTexture renderTexture)
			{
				float angle = 0f;
				Vector3 positionOffset = default(Vector3);
				if ((this.overrideHealthState ?? pawn.health.State) != PawnHealthState.Mobile && !pawn.ageTracker.CurLifeStage.alwaysDowned)
				{
					angle = 85f;
					positionOffset.x -= 0.18f;
					positionOffset.z -= 0.18f;
				}
				Find.PawnCacheRenderer.RenderPawn(pawn, renderTexture, this.cameraOffset, this.cameraZoom, angle, this.rotation, pawn.health.hediffSet.HasHead, this.renderHeadgear, this.renderClothes, true, positionOffset, this.overrideApparelColors, this.overrideHairColor, this.stylingStation);
			}

			// Token: 0x0400E2E3 RID: 58083
			public readonly Vector2 size;

			// Token: 0x0400E2E4 RID: 58084
			public readonly Vector3 cameraOffset;

			// Token: 0x0400E2E5 RID: 58085
			public readonly float cameraZoom;

			// Token: 0x0400E2E6 RID: 58086
			public readonly Rot4 rotation;

			// Token: 0x0400E2E7 RID: 58087
			public readonly bool renderHeadgear;

			// Token: 0x0400E2E8 RID: 58088
			public readonly bool renderClothes;

			// Token: 0x0400E2E9 RID: 58089
			public readonly bool stylingStation;

			// Token: 0x0400E2EA RID: 58090
			public readonly IReadOnlyDictionary<Apparel, Color> overrideApparelColors;

			// Token: 0x0400E2EB RID: 58091
			public readonly Color? overrideHairColor;

			// Token: 0x0400E2EC RID: 58092
			public readonly PawnHealthState? overrideHealthState;

			// Token: 0x0400E2ED RID: 58093
			private readonly int cachedHashCode;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000648 RID: 1608
	[StaticConstructorOnStartup]
	public static class GenUI
	{
		// Token: 0x0600325D RID: 12893 RVA: 0x0014103C File Offset: 0x0013F23C
		public static void SetLabelAlign(TextAnchor a)
		{
			Text.Anchor = a;
		}

		// Token: 0x0600325E RID: 12894 RVA: 0x00141044 File Offset: 0x0013F244
		public static void ResetLabelAlign()
		{
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x0600325F RID: 12895 RVA: 0x0014104C File Offset: 0x0013F24C
		public static float BackgroundDarkAlphaForText()
		{
			if (Find.CurrentMap == null)
			{
				return 0f;
			}
			float num = GenCelestial.CurCelestialSunGlow(Find.CurrentMap);
			float num2 = (Find.CurrentMap.Biome == BiomeDefOf.IceSheet) ? 1f : Mathf.Clamp01(Find.CurrentMap.snowGrid.TotalDepth / 1000f);
			return num * num2 * 0.41f;
		}

		// Token: 0x06003260 RID: 12896 RVA: 0x001410AC File Offset: 0x0013F2AC
		public static void DrawTextWinterShadow(Rect rect)
		{
			float num = GenUI.BackgroundDarkAlphaForText();
			if (num > 0.001f)
			{
				GUI.color = new Color(1f, 1f, 1f, num);
				GUI.DrawTexture(rect, GenUI.UnderShadowTex);
				GUI.color = Color.white;
			}
		}

		// Token: 0x06003261 RID: 12897 RVA: 0x001410F8 File Offset: 0x0013F2F8
		public static void DrawTextureWithMaterial(Rect rect, Texture texture, Material material, Rect texCoords = default(Rect))
		{
			if (texCoords == default(Rect))
			{
				if (material == null)
				{
					GUI.DrawTexture(rect, texture);
					return;
				}
				if (Event.current.type == EventType.repaint)
				{
					Color color = material.shader.SupportsMaskTex() ? GUI.color : new Color(GUI.color.r * 0.5f, GUI.color.g * 0.5f, GUI.color.b * 0.5f, GUI.color.a);
					Graphics.DrawTexture(rect, texture, new Rect(0f, 0f, 1f, 1f), 0, 0, 0, 0, color, material);
					return;
				}
			}
			else
			{
				if (material == null)
				{
					GUI.DrawTextureWithTexCoords(rect, texture, texCoords);
					return;
				}
				if (Event.current.type == EventType.repaint)
				{
					Color color2 = material.shader.SupportsMaskTex() ? GUI.color : new Color(GUI.color.r * 0.5f, GUI.color.g * 0.5f, GUI.color.b * 0.5f, GUI.color.a);
					Graphics.DrawTexture(rect, texture, texCoords, 0, 0, 0, 0, color2, material);
				}
			}
		}

		// Token: 0x06003262 RID: 12898 RVA: 0x00141238 File Offset: 0x0013F438
		public static void DrawTexturePartWithMaterial(Rect rect, Texture texture, Material material, Rect texCoords)
		{
			if (texCoords == default(Rect))
			{
				if (material == null)
				{
					GUI.DrawTexture(rect, texture);
					return;
				}
				if (Event.current.type == EventType.repaint)
				{
					Color color = material.shader.SupportsMaskTex() ? GUI.color : (GUI.color * 0.5f);
					Graphics.DrawTexture(rect, texture, new Rect(0f, 0f, 1f, 1f), 0, 0, 0, 0, color, material);
					return;
				}
			}
			else
			{
				texCoords.y = 1f - texCoords.y - texCoords.height;
				if (material == null)
				{
					GUI.DrawTextureWithTexCoords(rect, texture, texCoords);
					return;
				}
				if (Event.current.type == EventType.repaint)
				{
					Color color2 = material.shader.SupportsMaskTex() ? GUI.color : (GUI.color * 0.5f);
					Graphics.DrawTexture(rect, texture, texCoords, 0, 0, 0, 0, color2, material);
				}
			}
		}

		// Token: 0x06003263 RID: 12899 RVA: 0x00141330 File Offset: 0x0013F530
		public static float IconDrawScale(ThingDef tDef)
		{
			float num = tDef.uiIconScale;
			if (tDef.uiIconPath.NullOrEmpty() && tDef.graphicData != null)
			{
				num *= Mathf.Min(tDef.graphicData.drawSize.x / (float)tDef.Size.x, tDef.graphicData.drawSize.y / (float)tDef.Size.z);
			}
			return num;
		}

		// Token: 0x06003264 RID: 12900 RVA: 0x0014139C File Offset: 0x0013F59C
		public static void ErrorDialog(string message)
		{
			if (Find.WindowStack != null)
			{
				Find.WindowStack.Add(new Dialog_MessageBox(message, null, null, null, null, null, false, null, null, WindowLayer.Dialog));
			}
		}

		// Token: 0x06003265 RID: 12901 RVA: 0x001413D0 File Offset: 0x0013F5D0
		public static void DrawFlash(float centerX, float centerY, float size, float alpha, Color color)
		{
			Rect position = new Rect(centerX - size / 2f, centerY - size / 2f, size, size);
			Color color2 = color;
			color2.a = alpha;
			GUI.color = color2;
			GUI.DrawTexture(position, GenUI.UIFlash);
			GUI.color = Color.white;
		}

		// Token: 0x06003266 RID: 12902 RVA: 0x0014141C File Offset: 0x0013F61C
		public static Vector2 GetSizeCached(this string s)
		{
			if (GenUI.labelWidthCache.Count > 2000 || (Time.frameCount % 40000 == 0 && GenUI.labelWidthCache.Count > 100))
			{
				GenUI.labelWidthCache.Clear();
			}
			s = s.StripTags();
			Vector2 vector;
			if (!GenUI.labelWidthCache.TryGetValue(s, out vector))
			{
				vector = Text.CalcSize(s);
				GenUI.labelWidthCache.Add(s, vector);
			}
			return vector;
		}

		// Token: 0x06003267 RID: 12903 RVA: 0x0014148A File Offset: 0x0013F68A
		public static float GetWidthCached(this string s)
		{
			return s.GetSizeCached().x;
		}

		// Token: 0x06003268 RID: 12904 RVA: 0x00141497 File Offset: 0x0013F697
		public static float GetHeightCached(this string s)
		{
			return s.GetSizeCached().y;
		}

		// Token: 0x06003269 RID: 12905 RVA: 0x001414A4 File Offset: 0x0013F6A4
		public static void ClearLabelWidthCache()
		{
			GenUI.labelWidthCache.Clear();
		}

		// Token: 0x0600326A RID: 12906 RVA: 0x001414B0 File Offset: 0x0013F6B0
		public static Rect Rounded(this Rect r)
		{
			return new Rect((float)((int)r.x), (float)((int)r.y), (float)((int)r.width), (float)((int)r.height));
		}

		// Token: 0x0600326B RID: 12907 RVA: 0x001414DB File Offset: 0x0013F6DB
		public static Rect RoundedCeil(this Rect r)
		{
			return new Rect((float)Mathf.CeilToInt(r.x), (float)Mathf.CeilToInt(r.y), (float)Mathf.CeilToInt(r.width), (float)Mathf.CeilToInt(r.height));
		}

		// Token: 0x0600326C RID: 12908 RVA: 0x00141516 File Offset: 0x0013F716
		public static Vector2 Rounded(this Vector2 v)
		{
			return new Vector2((float)((int)v.x), (float)((int)v.y));
		}

		// Token: 0x0600326D RID: 12909 RVA: 0x00141530 File Offset: 0x0013F730
		public static float DistFromRect(Rect r, Vector2 p)
		{
			float num = Mathf.Abs(p.x - r.center.x) - r.width / 2f;
			if (num < 0f)
			{
				num = 0f;
			}
			float num2 = Mathf.Abs(p.y - r.center.y) - r.height / 2f;
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			return Mathf.Sqrt(num * num + num2 * num2);
		}

		// Token: 0x0600326E RID: 12910 RVA: 0x001415B4 File Offset: 0x0013F7B4
		public static void DrawMouseAttachment(Texture iconTex, string text = "", float angle = 0f, Vector2 offset = default(Vector2), Rect? customRect = null, Color? textColor = null, bool drawTextBackground = false, Color textBgColor = default(Color), Color? iconColor = null, Action<Rect> postDrawAction = null)
		{
			Vector2 mousePosition = Event.current.mousePosition;
			float num = mousePosition.y + 12f;
			if (drawTextBackground && text != "")
			{
				Rect value;
				if (customRect != null)
				{
					value = customRect.Value;
				}
				else
				{
					float num2 = Mathf.Min(Text.CalcSize(text).x, 200f);
					float height = Text.CalcHeight(text, 200f);
					float num3 = (iconTex != null) ? 42f : 0f;
					value = new Rect(mousePosition.x + 12f - 4f, num + num3, num2 + 8f, height);
				}
				Widgets.DrawBoxSolid(value, textBgColor);
			}
			if (iconTex != null)
			{
				Rect mouseRect;
				if (customRect != null)
				{
					mouseRect = customRect.Value;
				}
				else
				{
					mouseRect = new Rect(mousePosition.x + 8f, num + 8f, 32f, 32f);
				}
				Find.WindowStack.ImmediateWindow(34003428, mouseRect, WindowLayer.Super, delegate
				{
					Rect rect = mouseRect.AtZero();
					rect.position += new Vector2(offset.x * rect.size.x, offset.y * rect.size.y);
					GUI.color = (iconColor ?? Color.white);
					Widgets.DrawTextureRotated(rect, iconTex, angle, null);
					GUI.color = Color.white;
					Action<Rect> postDrawAction2 = postDrawAction;
					if (postDrawAction2 == null)
					{
						return;
					}
					postDrawAction2(rect);
				}, false, false, 0f, null, false);
				num += mouseRect.height + 10f;
			}
			if (text != "")
			{
				Rect textRect = new Rect(mousePosition.x + 12f, num, 200f, 9999f);
				Find.WindowStack.ImmediateWindow(34003429, textRect, WindowLayer.Super, delegate
				{
					GameFont font = Text.Font;
					Text.Font = GameFont.Small;
					GUI.color = (textColor ?? Color.white);
					Widgets.Label(textRect.AtZero(), text);
					GUI.color = Color.white;
					Text.Font = font;
				}, false, false, 0f, null, false);
			}
		}

		// Token: 0x0600326F RID: 12911 RVA: 0x001417D0 File Offset: 0x0013F9D0
		public static void DrawMouseAttachment(Texture2D icon)
		{
			Vector2 mousePosition = Event.current.mousePosition;
			Rect mouseRect = new Rect(mousePosition.x + 8f, mousePosition.y + 8f, 32f, 32f);
			Find.WindowStack.ImmediateWindow(34003428, mouseRect, WindowLayer.Super, delegate
			{
				GUI.DrawTexture(mouseRect.AtZero(), icon);
			}, false, false, 0f, null, false);
		}

		// Token: 0x06003270 RID: 12912 RVA: 0x00141850 File Offset: 0x0013FA50
		public static void RenderMouseoverBracket()
		{
			Vector3 position = UI.MouseCell().ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, GenUI.MouseoverBracketMaterial, 0);
		}

		// Token: 0x06003271 RID: 12913 RVA: 0x00141884 File Offset: 0x0013FA84
		public static void DrawStatusLevel(Need status, Rect rect)
		{
			Widgets.BeginGroup(rect);
			Widgets.Label(new Rect(0f, 2f, rect.width, 25f), status.LabelCap);
			Rect rect2 = new Rect(100f, 3f, GenUI.PieceBarSize.x, GenUI.PieceBarSize.y);
			Widgets.FillableBar(rect2, status.CurLevelPercentage);
			Widgets.FillableBarChangeArrows(rect2, status.GUIChangeArrow);
			Widgets.EndGroup();
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, status.GetTipString());
			}
			if (Mouse.IsOver(rect))
			{
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
		}

		// Token: 0x06003272 RID: 12914 RVA: 0x00141929 File Offset: 0x0013FB29
		public static IEnumerable<LocalTargetInfo> TargetsAtMouse(TargetingParameters clickParams, bool thingsOnly = false, ITargetingSource source = null)
		{
			return GenUI.TargetsAt(UI.MouseMapPosition(), clickParams, thingsOnly, source);
		}

		// Token: 0x06003273 RID: 12915 RVA: 0x00141938 File Offset: 0x0013FB38
		public static IEnumerable<LocalTargetInfo> TargetsAt(Vector3 clickPos, TargetingParameters clickParams, bool thingsOnly = false, ITargetingSource source = null)
		{
			GenUI.<TargetsAt>d__48 <TargetsAt>d__ = new GenUI.<TargetsAt>d__48(-2);
			<TargetsAt>d__.<>3__clickPos = clickPos;
			<TargetsAt>d__.<>3__clickParams = clickParams;
			<TargetsAt>d__.<>3__thingsOnly = thingsOnly;
			<TargetsAt>d__.<>3__source = source;
			return <TargetsAt>d__;
		}

		// Token: 0x06003274 RID: 12916 RVA: 0x00141960 File Offset: 0x0013FB60
		public static List<Thing> ThingsUnderMouse(Vector3 clickPos, float pawnWideClickRadius, TargetingParameters clickParams, ITargetingSource source = null)
		{
			IntVec3 intVec = IntVec3.FromVector3(clickPos);
			List<Thing> list = new List<Thing>();
			IReadOnlyList<Pawn> allPawnsSpawned = Find.CurrentMap.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				Pawn pawn = allPawnsSpawned[i];
				if ((pawn.DrawPos - clickPos).MagnitudeHorizontal() < 0.4f && clickParams.CanTarget(pawn, source))
				{
					list.Add(pawn);
					list.AddRange(ContainingSelectionUtility.SelectableContainedThings(pawn));
				}
			}
			list.Sort(new Comparison<Thing>(GenUI.CompareThingsByDistanceToMousePointer));
			GenUI.cellThings.Clear();
			foreach (Thing thing4 in Find.CurrentMap.thingGrid.ThingsAt(intVec))
			{
				if (!list.Contains(thing4) && clickParams.CanTarget(thing4, source))
				{
					GenUI.cellThings.Add(thing4);
					GenUI.cellThings.AddRange(ContainingSelectionUtility.SelectableContainedThings(thing4));
				}
			}
			IntVec3[] adjacentCells = GenAdj.AdjacentCells;
			for (int j = 0; j < adjacentCells.Length; j++)
			{
				IntVec3 c = adjacentCells[j] + intVec;
				if (c.InBounds(Find.CurrentMap) && c.GetItemCount(Find.CurrentMap) > 1)
				{
					foreach (Thing thing2 in Find.CurrentMap.thingGrid.ThingsAt(c))
					{
						if (thing2.def.category == ThingCategory.Item && (thing2.TrueCenter() - UI.MouseMapPosition()).MagnitudeHorizontalSquared() <= 0.25f && !list.Contains(thing2) && clickParams.CanTarget(thing2, source))
						{
							GenUI.cellThings.Add(thing2);
						}
					}
				}
			}
			List<Thing> list2 = Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.WithCustomRectForSelector);
			for (int k = 0; k < list2.Count; k++)
			{
				Thing thing3 = list2[k];
				if (thing3.CustomRectForSelector != null && thing3.CustomRectForSelector.Value.Contains(intVec) && !list.Contains(thing3) && clickParams.CanTarget(thing3, source))
				{
					GenUI.cellThings.Add(thing3);
				}
			}
			GenUI.cellThings.Sort(new Comparison<Thing>(GenUI.CompareThingsByDrawAltitudeOrDistToItem));
			list.AddRange(GenUI.cellThings);
			GenUI.cellThings.Clear();
			IReadOnlyList<Pawn> allPawnsSpawned2 = Find.CurrentMap.mapPawns.AllPawnsSpawned;
			for (int l = 0; l < allPawnsSpawned2.Count; l++)
			{
				Pawn pawn2 = allPawnsSpawned2[l];
				if ((pawn2.DrawPos - clickPos).MagnitudeHorizontal() < pawnWideClickRadius && clickParams.CanTarget(pawn2, source))
				{
					GenUI.cellThings.Add(pawn2);
				}
			}
			GenUI.cellThings.Sort(new Comparison<Thing>(GenUI.CompareThingsByDistanceToMousePointer));
			for (int m = 0; m < GenUI.cellThings.Count; m++)
			{
				if (!list.Contains(GenUI.cellThings[m]))
				{
					list.Add(GenUI.cellThings[m]);
					list.AddRange(ContainingSelectionUtility.SelectableContainedThings(GenUI.cellThings[m]));
				}
			}
			list.RemoveAll((Thing thing) => !clickParams.CanTarget(thing, source));
			list.RemoveAll(delegate(Thing thing)
			{
				Pawn pawn3 = thing as Pawn;
				return pawn3 != null && pawn3.IsHiddenFromPlayer();
			});
			GenUI.cellThings.Clear();
			return list;
		}

		// Token: 0x06003275 RID: 12917 RVA: 0x00141D7C File Offset: 0x0013FF7C
		private static int CompareThingsByDistanceToMousePointer(Thing a, Thing b)
		{
			Vector3 b2 = UI.MouseMapPosition();
			float num = (a.DrawPosHeld.Value - b2).MagnitudeHorizontalSquared();
			float num2 = (b.DrawPosHeld.Value - b2).MagnitudeHorizontalSquared();
			if (num < num2)
			{
				return -1;
			}
			if (num == num2)
			{
				return b.Spawned.CompareTo(a.Spawned);
			}
			return 1;
		}

		// Token: 0x06003276 RID: 12918 RVA: 0x00141DE4 File Offset: 0x0013FFE4
		private static int CompareThingsByDrawAltitudeOrDistToItem(Thing A, Thing B)
		{
			if (A.def.category == ThingCategory.Item && B.def.category == ThingCategory.Item)
			{
				return (A.TrueCenter() - UI.MouseMapPosition()).MagnitudeHorizontalSquared().CompareTo((B.TrueCenter() - UI.MouseMapPosition()).MagnitudeHorizontalSquared());
			}
			Thing spawnedParentOrMe = A.SpawnedParentOrMe;
			Thing spawnedParentOrMe2 = B.SpawnedParentOrMe;
			if (spawnedParentOrMe.def.Altitude != spawnedParentOrMe2.def.Altitude)
			{
				return spawnedParentOrMe2.def.Altitude.CompareTo(spawnedParentOrMe.def.Altitude);
			}
			return B.Spawned.CompareTo(A.Spawned);
		}

		// Token: 0x06003277 RID: 12919 RVA: 0x00141E99 File Offset: 0x00140099
		public static int CurrentAdjustmentMultiplier()
		{
			if (KeyBindingDefOf.ModifierIncrement_10x.IsDownEvent && KeyBindingDefOf.ModifierIncrement_100x.IsDownEvent)
			{
				return 1000;
			}
			if (KeyBindingDefOf.ModifierIncrement_100x.IsDownEvent)
			{
				return 100;
			}
			if (KeyBindingDefOf.ModifierIncrement_10x.IsDownEvent)
			{
				return 10;
			}
			return 1;
		}

		// Token: 0x06003278 RID: 12920 RVA: 0x00141ED8 File Offset: 0x001400D8
		public static Rect GetInnerRect(this Rect rect)
		{
			return rect.ContractedBy(17f);
		}

		// Token: 0x06003279 RID: 12921 RVA: 0x00141EE5 File Offset: 0x001400E5
		public static Rect ExpandedBy(this Rect rect, float margin)
		{
			return new Rect(rect.x - margin, rect.y - margin, rect.width + margin * 2f, rect.height + margin * 2f);
		}

		// Token: 0x0600327A RID: 12922 RVA: 0x00141F1C File Offset: 0x0014011C
		public static Rect ExpandedBy(this Rect rect, float marginX, float marginY)
		{
			return new Rect(rect.x - marginX, rect.y - marginY, rect.width + marginX * 2f, rect.height + marginY * 2f);
		}

		// Token: 0x0600327B RID: 12923 RVA: 0x00141F53 File Offset: 0x00140153
		public static Rect ContractedBy(this Rect rect, float margin)
		{
			return new Rect(rect.x + margin, rect.y + margin, rect.width - margin * 2f, rect.height - margin * 2f);
		}

		// Token: 0x0600327C RID: 12924 RVA: 0x00141F8A File Offset: 0x0014018A
		public static Rect ContractedBy(this Rect rect, float marginX, float marginY)
		{
			return new Rect(rect.x + marginX, rect.y + marginY, rect.width - marginX * 2f, rect.height - marginY * 2f);
		}

		// Token: 0x0600327D RID: 12925 RVA: 0x00141FC4 File Offset: 0x001401C4
		public static Rect ScaledBy(this Rect rect, float scale)
		{
			rect.x -= rect.width * (scale - 1f) / 2f;
			rect.y -= rect.height * (scale - 1f) / 2f;
			rect.width *= scale;
			rect.height *= scale;
			return rect;
		}

		// Token: 0x0600327E RID: 12926 RVA: 0x00142036 File Offset: 0x00140236
		public static Rect CenteredOnXIn(this Rect rect, Rect otherRect)
		{
			return new Rect(otherRect.x + (otherRect.width - rect.width) / 2f, rect.y, rect.width, rect.height);
		}

		// Token: 0x0600327F RID: 12927 RVA: 0x0014206F File Offset: 0x0014026F
		public static Rect CenteredOnYIn(this Rect rect, Rect otherRect)
		{
			return new Rect(rect.x, otherRect.y + (otherRect.height - rect.height) / 2f, rect.width, rect.height);
		}

		// Token: 0x06003280 RID: 12928 RVA: 0x001420A8 File Offset: 0x001402A8
		public static Rect AtZero(this Rect rect)
		{
			return new Rect(0f, 0f, rect.width, rect.height);
		}

		// Token: 0x06003281 RID: 12929 RVA: 0x001420C8 File Offset: 0x001402C8
		public static Rect Union(this Rect a, Rect b)
		{
			return new Rect
			{
				min = Vector2.Min(a.min, b.min),
				max = Vector2.Max(a.max, b.max)
			};
		}

		// Token: 0x06003282 RID: 12930 RVA: 0x00142112 File Offset: 0x00140312
		public static void AbsorbClicksInRect(Rect r)
		{
			if (Find.WindowStack != null && Find.WindowStack.MouseObscuredNow)
			{
				return;
			}
			if (Event.current.type == EventType.mouseDown && r.Contains(Event.current.mousePosition))
			{
				Event.current.Use();
			}
		}

		// Token: 0x06003283 RID: 12931 RVA: 0x00142151 File Offset: 0x00140351
		public static Rect LeftHalf(this Rect rect)
		{
			return new Rect(rect.x, rect.y, rect.width / 2f, rect.height);
		}

		// Token: 0x06003284 RID: 12932 RVA: 0x0014217A File Offset: 0x0014037A
		public static Rect LeftPart(this Rect rect, float pct)
		{
			return new Rect(rect.x, rect.y, rect.width * pct, rect.height);
		}

		// Token: 0x06003285 RID: 12933 RVA: 0x0014219F File Offset: 0x0014039F
		public static Rect LeftPartPixels(this Rect rect, float width)
		{
			return new Rect(rect.x, rect.y, width, rect.height);
		}

		// Token: 0x06003286 RID: 12934 RVA: 0x001421BC File Offset: 0x001403BC
		public static Rect RightHalf(this Rect rect)
		{
			return new Rect(rect.x + rect.width / 2f, rect.y, rect.width / 2f, rect.height);
		}

		// Token: 0x06003287 RID: 12935 RVA: 0x001421F3 File Offset: 0x001403F3
		public static Rect RightPart(this Rect rect, float pct)
		{
			return new Rect(rect.x + rect.width * (1f - pct), rect.y, rect.width * pct, rect.height);
		}

		// Token: 0x06003288 RID: 12936 RVA: 0x00142228 File Offset: 0x00140428
		public static Rect RightPartPixels(this Rect rect, float width)
		{
			return new Rect(rect.x + rect.width - width, rect.y, width, rect.height);
		}

		// Token: 0x06003289 RID: 12937 RVA: 0x0014224F File Offset: 0x0014044F
		public static Rect TopHalf(this Rect rect)
		{
			return new Rect(rect.x, rect.y, rect.width, rect.height / 2f);
		}

		// Token: 0x0600328A RID: 12938 RVA: 0x00142278 File Offset: 0x00140478
		public static Rect TopPart(this Rect rect, float pct)
		{
			return new Rect(rect.x, rect.y, rect.width, rect.height * pct);
		}

		// Token: 0x0600328B RID: 12939 RVA: 0x0014229D File Offset: 0x0014049D
		public static Rect TopPartPixels(this Rect rect, float height)
		{
			return new Rect(rect.x, rect.y, rect.width, height);
		}

		// Token: 0x0600328C RID: 12940 RVA: 0x001422BA File Offset: 0x001404BA
		public static Rect BottomHalf(this Rect rect)
		{
			return new Rect(rect.x, rect.y + rect.height / 2f, rect.width, rect.height / 2f);
		}

		// Token: 0x0600328D RID: 12941 RVA: 0x001422F1 File Offset: 0x001404F1
		public static Rect BottomPart(this Rect rect, float pct)
		{
			return new Rect(rect.x, rect.y + rect.height * (1f - pct), rect.width, rect.height * pct);
		}

		// Token: 0x0600328E RID: 12942 RVA: 0x00142326 File Offset: 0x00140526
		public static Rect BottomPartPixels(this Rect rect, float height)
		{
			return new Rect(rect.x, rect.y + rect.height - height, rect.width, height);
		}

		// Token: 0x0600328F RID: 12943 RVA: 0x00142350 File Offset: 0x00140550
		public static Rect MiddlePart(this Rect rect, float pctWidth, float pctHeight)
		{
			return new Rect(rect.x + rect.width / 2f - rect.width * pctWidth / 2f, rect.y + rect.height / 2f - rect.height * pctHeight / 2f, rect.width * pctWidth, rect.height * pctHeight);
		}

		// Token: 0x06003290 RID: 12944 RVA: 0x001423C0 File Offset: 0x001405C0
		public static Rect MiddlePartPixels(this Rect rect, float width, float height)
		{
			return new Rect(rect.x + rect.width / 2f - width / 2f, rect.y + rect.height / 2f - height / 2f, width, height);
		}

		// Token: 0x06003291 RID: 12945 RVA: 0x0014240E File Offset: 0x0014060E
		public static IEnumerable<Vector2> Corners(this Rect rect)
		{
			GenUI.<Corners>d__80 <Corners>d__ = new GenUI.<Corners>d__80(-2);
			<Corners>d__.<>3__rect = rect;
			return <Corners>d__;
		}

		// Token: 0x06003292 RID: 12946 RVA: 0x00142420 File Offset: 0x00140620
		public static bool SplitHorizontallyWithMargin(this Rect rect, out Rect top, out Rect bottom, out float overflow, float compressibleMargin = 0f, float? topHeight = null, float? bottomHeight = null)
		{
			if (topHeight != null == (bottomHeight != null))
			{
				throw new ArgumentException("Exactly one null height and one non-null height must be provided.");
			}
			overflow = Mathf.Max(0f, (topHeight ?? bottomHeight.GetValueOrDefault()) - rect.height);
			float height = Mathf.Clamp(topHeight ?? (rect.height - bottomHeight.Value - compressibleMargin), 0f, rect.height);
			float num = Mathf.Clamp(bottomHeight ?? (rect.height - topHeight.Value - compressibleMargin), 0f, rect.height);
			top = new Rect(rect.x, rect.y, rect.width, height);
			bottom = new Rect(rect.x, rect.yMax - num, rect.width, num);
			return overflow == 0f;
		}

		// Token: 0x06003293 RID: 12947 RVA: 0x00142538 File Offset: 0x00140738
		public static void SplitVerticallyWithMargin(this Rect rect, out Rect left, out Rect right, float margin)
		{
			float num = rect.width / 2f;
			left = new Rect(rect.x, rect.y, num - margin / 2f, rect.height);
			right = new Rect(left.xMax + margin, rect.y, num - margin / 2f, rect.height);
		}

		// Token: 0x06003294 RID: 12948 RVA: 0x001425A8 File Offset: 0x001407A8
		public static bool SplitVerticallyWithMargin(this Rect rect, out Rect left, out Rect right, out float overflow, float compressibleMargin = 0f, float? leftWidth = null, float? rightWidth = null)
		{
			if (leftWidth != null == (rightWidth != null))
			{
				throw new ArgumentException("Exactly one null width and one non-null width must be provided.");
			}
			overflow = Mathf.Max(0f, (leftWidth ?? rightWidth.GetValueOrDefault()) - rect.width);
			float width = Mathf.Clamp(leftWidth ?? (rect.width - rightWidth.Value - compressibleMargin), 0f, rect.width);
			float num = Mathf.Clamp(rightWidth ?? (rect.width - leftWidth.Value - compressibleMargin), 0f, rect.width);
			left = new Rect(rect.x, rect.y, width, rect.height);
			right = new Rect(rect.xMax - num, rect.y, num, rect.height);
			return overflow == 0f;
		}

		// Token: 0x06003295 RID: 12949 RVA: 0x001426C0 File Offset: 0x001408C0
		public static void SplitHorizontally(this Rect rect, float topHeight, out Rect top, out Rect bottom)
		{
			float num;
			rect.SplitHorizontallyWithMargin(out top, out bottom, out num, 0f, new float?(topHeight), null);
		}

		// Token: 0x06003296 RID: 12950 RVA: 0x001426EC File Offset: 0x001408EC
		public static void SplitVertically(this Rect rect, float leftWidth, out Rect left, out Rect right)
		{
			float num;
			rect.SplitVerticallyWithMargin(out left, out right, out num, 0f, new float?(leftWidth), null);
		}

		// Token: 0x06003297 RID: 12951 RVA: 0x00142718 File Offset: 0x00140918
		public static Color LerpColor(List<Pair<float, Color>> colors, float value)
		{
			if (colors.Count == 0)
			{
				return Color.white;
			}
			int i = 0;
			while (i < colors.Count)
			{
				if (value < colors[i].First)
				{
					if (i == 0)
					{
						return colors[i].Second;
					}
					return Color.Lerp(colors[i - 1].Second, colors[i].Second, Mathf.InverseLerp(colors[i - 1].First, colors[i].First, value));
				}
				else
				{
					i++;
				}
			}
			return colors.Last<Pair<float, Color>>().Second;
		}

		// Token: 0x06003298 RID: 12952 RVA: 0x001427C4 File Offset: 0x001409C4
		public static Vector2 GetMouseAttachedWindowPos(float width, float height)
		{
			Vector2 mousePosition = Event.current.mousePosition;
			float y;
			if (mousePosition.y + 14f + height < (float)UI.screenHeight)
			{
				y = mousePosition.y + 14f;
			}
			else if (mousePosition.y - 5f - height >= 0f)
			{
				y = mousePosition.y - 5f - height;
			}
			else
			{
				y = (float)UI.screenHeight - (14f + height);
			}
			float x;
			if (mousePosition.x + 16f + width < (float)UI.screenWidth)
			{
				x = mousePosition.x + 16f;
			}
			else
			{
				x = mousePosition.x - 4f - width;
			}
			return new Vector2(x, y);
		}

		// Token: 0x06003299 RID: 12953 RVA: 0x00142880 File Offset: 0x00140A80
		public static float GetCenteredButtonPos(int buttonIndex, int buttonsCount, float totalWidth, float buttonWidth, float pad = 10f)
		{
			float num = (float)buttonsCount * buttonWidth + (float)(buttonsCount - 1) * pad;
			return Mathf.Floor((totalWidth - num) / 2f + (float)buttonIndex * (buttonWidth + pad));
		}

		// Token: 0x0600329A RID: 12954 RVA: 0x001428B0 File Offset: 0x00140AB0
		public static void DrawArrowPointingAt(Rect rect)
		{
			Vector2 vector = new Vector2((float)UI.screenWidth, (float)UI.screenHeight) / 2f;
			float angle = Mathf.Atan2(rect.center.x - vector.x, vector.y - rect.center.y) * 57.29578f;
			Vector2 vector2 = new Bounds(rect.center, rect.size).ClosestPoint(vector);
			Rect position = new Rect(vector2 + Vector2.left * (float)GenUI.ArrowTex.width * 0.5f, new Vector2((float)GenUI.ArrowTex.width, (float)GenUI.ArrowTex.height));
			Matrix4x4 matrix = GUI.matrix;
			GUI.matrix = Matrix4x4.identity;
			Vector2 center = GUIUtility.GUIToScreenPoint(vector2);
			GUI.matrix = matrix;
			UI.RotateAroundPivot(angle, center);
			GUI.DrawTexture(position, GenUI.ArrowTex);
			GUI.matrix = matrix;
			UnityGUIBugsFixer.Notify_GUIMatrixChanged();
		}

		// Token: 0x0600329B RID: 12955 RVA: 0x001429C0 File Offset: 0x00140BC0
		public static void DrawArrowPointingAtWorldspace(Vector3 worldspace, Camera camera)
		{
			Vector3 vector = camera.WorldToScreenPoint(worldspace) / Prefs.UIScale;
			GenUI.DrawArrowPointingAt(new Rect(new Vector2(vector.x, (float)UI.screenHeight - vector.y) + new Vector2(-2f, 2f), new Vector2(4f, 4f)));
		}

		// Token: 0x0600329C RID: 12956 RVA: 0x00142A24 File Offset: 0x00140C24
		public static Rect DrawElementStack<T>(Rect rect, float rowHeight, List<T> elements, GenUI.StackElementDrawer<T> drawer, GenUI.StackElementWidthGetter<T> widthGetter, float rowMargin = 4f, float elementMargin = 5f, bool allowOrderOptimization = true)
		{
			GenUI.tmpRects.Clear();
			GenUI.tmpRects2.Clear();
			for (int i = 0; i < elements.Count; i++)
			{
				GenUI.tmpRects.Add(new GenUI.StackedElementRect(new Rect(0f, 0f, widthGetter(elements[i]), rowHeight), i));
			}
			int num = Mathf.FloorToInt(rect.height / rowHeight);
			List<GenUI.StackedElementRect> list = GenUI.tmpRects;
			float num3;
			float num2;
			if (allowOrderOptimization)
			{
				num2 = (num3 = 0f);
				while (num2 < (float)num)
				{
					GenUI.StackedElementRect item = default(GenUI.StackedElementRect);
					int num4 = -1;
					for (int j = 0; j < list.Count; j++)
					{
						GenUI.StackedElementRect stackedElementRect = list[j];
						if (num4 == -1 || (item.rect.width < stackedElementRect.rect.width && stackedElementRect.rect.width < rect.width - num3))
						{
							num4 = j;
							item = stackedElementRect;
						}
					}
					if (num4 == -1)
					{
						if (num3 == 0f)
						{
							break;
						}
						num3 = 0f;
						num2 += 1f;
					}
					else
					{
						num3 += item.rect.width + elementMargin;
						GenUI.tmpRects2.Add(item);
					}
					list.RemoveAt(num4);
					if (list.Count <= 0)
					{
						break;
					}
				}
				list = GenUI.tmpRects2;
			}
			num2 = (num3 = 0f);
			while (list.Count > 0)
			{
				GenUI.StackedElementRect stackedElementRect2 = list[0];
				if (num3 + stackedElementRect2.rect.width > rect.width)
				{
					num3 = 0f;
					num2 += rowHeight + rowMargin;
				}
				if (drawer != null)
				{
					drawer(new Rect(rect.x + num3, rect.y + num2, stackedElementRect2.rect.width, stackedElementRect2.rect.height), elements[stackedElementRect2.elementIndex]);
				}
				num3 += stackedElementRect2.rect.width + elementMargin;
				list.RemoveAt(0);
			}
			return new Rect(rect.x, rect.y, rect.width, num2 + rowHeight);
		}

		// Token: 0x0600329D RID: 12957 RVA: 0x00142C38 File Offset: 0x00140E38
		public static Rect DrawElementStackVertical<T>(Rect rect, float rowHeight, List<T> elements, GenUI.StackElementDrawer<T> drawer, GenUI.StackElementWidthGetter<T> widthGetter, float elementMargin = 5f, bool highlightOdd = false)
		{
			GenUI.tmpRects.Clear();
			for (int i = 0; i < elements.Count; i++)
			{
				GenUI.tmpRects.Add(new GenUI.StackedElementRect(new Rect(0f, 0f, widthGetter(elements[i]), rowHeight), i));
			}
			int elem = Mathf.FloorToInt(rect.height / rowHeight);
			GenUI.spacingCache.Reset(elem);
			int num = 0;
			float num2 = 0f;
			float num3 = 0f;
			for (int j = 0; j < GenUI.tmpRects.Count; j++)
			{
				GenUI.StackedElementRect stackedElementRect = GenUI.tmpRects[j];
				if (num3 + stackedElementRect.rect.height > rect.height)
				{
					num3 = 0f;
					num = 0;
				}
				Rect rect2 = new Rect(rect.x + GenUI.spacingCache.GetSpaceFor(num), rect.y + num3, stackedElementRect.rect.width, stackedElementRect.rect.height);
				if (highlightOdd && j % 2 == 1)
				{
					Widgets.DrawLightHighlight(rect2);
				}
				drawer(rect2, elements[stackedElementRect.elementIndex]);
				num3 += stackedElementRect.rect.height + elementMargin;
				GenUI.spacingCache.AddSpace(num, stackedElementRect.rect.width + elementMargin);
				num2 = Mathf.Max(num2, GenUI.spacingCache.GetSpaceFor(num));
				num++;
			}
			return new Rect(rect.x, rect.y, num2, num3 + rowHeight);
		}

		// Token: 0x04002039 RID: 8249
		public const float Pad = 10f;

		// Token: 0x0400203A RID: 8250
		public const float GapTiny = 4f;

		// Token: 0x0400203B RID: 8251
		public const float GapSmall = 10f;

		// Token: 0x0400203C RID: 8252
		public const float Gap = 17f;

		// Token: 0x0400203D RID: 8253
		public const float GapWide = 26f;

		// Token: 0x0400203E RID: 8254
		public const float GapLabel = 22f;

		// Token: 0x0400203F RID: 8255
		public const float ListSpacing = 28f;

		// Token: 0x04002040 RID: 8256
		public const float MouseAttachIconSize = 32f;

		// Token: 0x04002041 RID: 8257
		public const float MouseAttachIconOffset = 8f;

		// Token: 0x04002042 RID: 8258
		public const float ScrollBarWidth = 16f;

		// Token: 0x04002043 RID: 8259
		public const float HorizontalSliderHeight = 10f;

		// Token: 0x04002044 RID: 8260
		public static readonly Vector2 TradeableDrawSize = new Vector2(150f, 45f);

		// Token: 0x04002045 RID: 8261
		public static readonly Color MouseoverColor = new Color(0.3f, 0.7f, 0.9f);

		// Token: 0x04002046 RID: 8262
		public static readonly Color SubtleMouseoverColor = new Color(0.7f, 0.7f, 0.7f);

		// Token: 0x04002047 RID: 8263
		public static readonly Color FillableBar_Green = new Color(0.40392157f, 0.7019608f, 0.28627452f);

		// Token: 0x04002048 RID: 8264
		public static readonly Color FillableBar_Empty = new Color(0.03f, 0.035f, 0.05f);

		// Token: 0x04002049 RID: 8265
		public static readonly Vector2 MaxWinSize = new Vector2(1010f, 754f);

		// Token: 0x0400204A RID: 8266
		public const float SmallIconSize = 24f;

		// Token: 0x0400204B RID: 8267
		public const int RootGUIDepth = 50;

		// Token: 0x0400204C RID: 8268
		private const float MouseIconSize = 32f;

		// Token: 0x0400204D RID: 8269
		private const float MouseIconOffset = 12f;

		// Token: 0x0400204E RID: 8270
		private static readonly Material MouseoverBracketMaterial = MaterialPool.MatFrom("UI/Overlays/MouseoverBracketTex", ShaderDatabase.MetaOverlay);

		// Token: 0x0400204F RID: 8271
		private static readonly Texture2D UnderShadowTex = ContentFinder<Texture2D>.Get("UI/Misc/ScreenCornerShadow", true);

		// Token: 0x04002050 RID: 8272
		private static readonly Texture2D UIFlash = ContentFinder<Texture2D>.Get("UI/Misc/Flash", true);

		// Token: 0x04002051 RID: 8273
		private static Dictionary<string, Vector2> labelWidthCache = new Dictionary<string, Vector2>();

		// Token: 0x04002052 RID: 8274
		private static readonly Vector2 PieceBarSize = new Vector2(100f, 17f);

		// Token: 0x04002053 RID: 8275
		public const float PawnDirectClickRadius = 0.4f;

		// Token: 0x04002054 RID: 8276
		private static List<Thing> cellThings = new List<Thing>(32);

		// Token: 0x04002055 RID: 8277
		private static readonly Texture2D ArrowTex = ContentFinder<Texture2D>.Get("UI/Overlays/Arrow", true);

		// Token: 0x04002056 RID: 8278
		private static List<GenUI.StackedElementRect> tmpRects = new List<GenUI.StackedElementRect>();

		// Token: 0x04002057 RID: 8279
		private static List<GenUI.StackedElementRect> tmpRects2 = new List<GenUI.StackedElementRect>();

		// Token: 0x04002058 RID: 8280
		public const float ElementStackDefaultElementMargin = 5f;

		// Token: 0x04002059 RID: 8281
		public const float ElementStackDefaultRowMargin = 4f;

		// Token: 0x0400205A RID: 8282
		private static GenUI.SpacingCache spacingCache;

		// Token: 0x020029E2 RID: 10722
		private struct StackedElementRect
		{
			// Token: 0x060104CA RID: 66762 RVA: 0x005C20DA File Offset: 0x005C02DA
			public StackedElementRect(Rect rect, int elementIndex)
			{
				this.rect = rect;
				this.elementIndex = elementIndex;
			}

			// Token: 0x0400ABDB RID: 43995
			public Rect rect;

			// Token: 0x0400ABDC RID: 43996
			public int elementIndex;
		}

		// Token: 0x020029E3 RID: 10723
		public class AnonymousStackElement
		{
			// Token: 0x0400ABDD RID: 43997
			public Action<Rect> drawer;

			// Token: 0x0400ABDE RID: 43998
			public float width;
		}

		// Token: 0x020029E4 RID: 10724
		private struct SpacingCache
		{
			// Token: 0x060104CC RID: 66764 RVA: 0x005C20F4 File Offset: 0x005C02F4
			public void Reset(int elem = 16)
			{
				if (this.spaces == null || this.maxElements != elem)
				{
					this.maxElements = elem;
					this.spaces = new float[this.maxElements];
					return;
				}
				for (int i = 0; i < this.maxElements; i++)
				{
					this.spaces[i] = 0f;
				}
			}

			// Token: 0x060104CD RID: 66765 RVA: 0x005C2149 File Offset: 0x005C0349
			public float GetSpaceFor(int elem)
			{
				if (this.spaces == null || this.maxElements < 1)
				{
					this.Reset(16);
				}
				if (elem >= 0 && elem < this.maxElements)
				{
					return this.spaces[elem];
				}
				return 0f;
			}

			// Token: 0x060104CE RID: 66766 RVA: 0x005C217F File Offset: 0x005C037F
			public void AddSpace(int elem, float space)
			{
				if (this.spaces == null || this.maxElements < 1)
				{
					this.Reset(16);
				}
				if (elem >= 0 && elem < this.maxElements)
				{
					this.spaces[elem] += space;
				}
			}

			// Token: 0x0400ABDF RID: 43999
			private int maxElements;

			// Token: 0x0400ABE0 RID: 44000
			private float[] spaces;
		}

		// Token: 0x020029E5 RID: 10725
		// (Invoke) Token: 0x060104D0 RID: 66768
		public delegate void StackElementDrawer<T>(Rect rect, T element);

		// Token: 0x020029E6 RID: 10726
		// (Invoke) Token: 0x060104D4 RID: 66772
		public delegate float StackElementWidthGetter<T>(T element);
	}
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using LudeonTK;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.Sound;
using Verse.Steam;

namespace Verse
{
	// Token: 0x0200065D RID: 1629
	[StaticConstructorOnStartup]
	public static class Widgets
	{
		// Token: 0x170009AC RID: 2476
		// (get) Token: 0x06003303 RID: 13059 RVA: 0x00145F40 File Offset: 0x00144140
		public static bool Painting
		{
			get
			{
				return Widgets.dropdownPainting || Widgets.checkboxPainting;
			}
		}

		// Token: 0x06003304 RID: 13060 RVA: 0x00145F50 File Offset: 0x00144150
		static Widgets()
		{
			Color color = new Color(1f, 1f, 1f, 0f);
			Widgets.LineTexAA = new Texture2D(1, 3, TextureFormat.ARGB32, false);
			Widgets.LineTexAA.name = "LineTexAA";
			Widgets.LineTexAA.SetPixel(0, 0, color);
			Widgets.LineTexAA.SetPixel(0, 1, Color.white);
			Widgets.LineTexAA.SetPixel(0, 2, color);
			Widgets.LineTexAA.Apply();
		}

		// Token: 0x06003305 RID: 13061 RVA: 0x00146587 File Offset: 0x00144787
		public static void BeginGroup(Rect rect)
		{
			GUI.BeginGroup(rect);
			UnityGUIBugsFixer.Notify_BeginGroup();
		}

		// Token: 0x06003306 RID: 13062 RVA: 0x00146594 File Offset: 0x00144794
		public static void EndGroup()
		{
			GUI.EndGroup();
			UnityGUIBugsFixer.Notify_EndGroup();
		}

		// Token: 0x06003307 RID: 13063 RVA: 0x001465A0 File Offset: 0x001447A0
		public static void ClearLabelCache()
		{
			Widgets.LabelCache.Clear();
		}

		// Token: 0x06003308 RID: 13064 RVA: 0x001465AC File Offset: 0x001447AC
		public static bool CanDrawIconFor(Def def)
		{
			BuildableDef buildableDef = def as BuildableDef;
			if (buildableDef != null)
			{
				return buildableDef.uiIcon != null;
			}
			FactionDef factionDef = def as FactionDef;
			return factionDef != null && factionDef.FactionIcon != null;
		}

		// Token: 0x06003309 RID: 13065 RVA: 0x001465E8 File Offset: 0x001447E8
		public static void DefIcon(Rect rect, Def def, ThingDef stuffDef = null, float scale = 1f, ThingStyleDef thingStyleDef = null, bool drawPlaceholder = false, Color? color = null, Material material = null, int? graphicIndexOverride = null, float alpha = 1f)
		{
			BuildableDef buildableDef = def as BuildableDef;
			if (buildableDef != null)
			{
				rect.position += new Vector2(buildableDef.uiIconOffset.x * rect.size.x, buildableDef.uiIconOffset.y * rect.size.y);
			}
			ThingDef thingDef = def as ThingDef;
			if (thingDef != null && thingDef.IsFrame && thingDef.entityDefToBuild != null)
			{
				def = thingDef.entityDefToBuild;
			}
			ThingDef thingDef2 = def as ThingDef;
			if (thingDef2 != null)
			{
				Widgets.ThingIcon(rect, thingDef2, stuffDef, thingStyleDef, scale, color, graphicIndexOverride, alpha);
				return;
			}
			PawnKindDef pawnKindDef = def as PawnKindDef;
			if (pawnKindDef != null)
			{
				Widgets.ThingIcon(rect, pawnKindDef.race, stuffDef, thingStyleDef, scale, color, graphicIndexOverride, alpha);
				return;
			}
			RecipeDef recipeDef = def as RecipeDef;
			if (recipeDef != null && (recipeDef.UIIconThing != null || recipeDef.UIIcon != null))
			{
				if (recipeDef.UIIconThing != null)
				{
					Widgets.ThingIcon(rect, recipeDef.UIIconThing, null, thingStyleDef, scale, color, graphicIndexOverride, alpha);
					return;
				}
				if (recipeDef.UIIcon != null)
				{
					Widgets.DrawTextureFitted(rect, recipeDef.UIIcon, scale, material, alpha);
					return;
				}
			}
			else
			{
				TerrainDef terrainDef = def as TerrainDef;
				if (terrainDef != null && terrainDef.uiIcon != null)
				{
					GUI.color = terrainDef.uiIconColor;
					Rect texCoords = terrainDef.cropIcon ? Widgets.CroppedTerrainTextureRect(terrainDef.uiIcon) : new Rect(0f, 0f, 1f, 1f);
					Widgets.DrawTextureFitted(rect, terrainDef.uiIcon, scale, Vector2.one, texCoords, 0f, material, alpha);
					GUI.color = Color.white;
					return;
				}
				FactionDef factionDef = def as FactionDef;
				if (factionDef != null)
				{
					if (!factionDef.colorSpectrum.NullOrEmpty<Color>())
					{
						GUI.color = factionDef.colorSpectrum.FirstOrDefault<Color>();
					}
					Widgets.DrawTextureFitted(rect, factionDef.FactionIcon, scale, material, alpha);
					GUI.color = Color.white;
					return;
				}
				StyleItemDef styleItemDef = def as StyleItemDef;
				if (styleItemDef != null)
				{
					Widgets.DrawTextureFitted(rect, styleItemDef.Icon, scale, material, alpha);
					return;
				}
				BodyTypeDef bodyTypeDef = def as BodyTypeDef;
				if (bodyTypeDef != null)
				{
					Widgets.DrawTextureFitted(rect, bodyTypeDef.Icon, scale, material, alpha);
					return;
				}
				HeadTypeDef headTypeDef = def as HeadTypeDef;
				if (headTypeDef != null)
				{
					Widgets.DrawTextureFitted(rect, headTypeDef.Icon, scale, material, alpha);
					return;
				}
				GeneDef geneDef = def as GeneDef;
				if (geneDef != null)
				{
					GUI.color = (color ?? geneDef.IconColor);
					Widgets.DrawTextureFitted(rect, geneDef.Icon, scale, material, alpha);
					GUI.color = Color.white;
					return;
				}
				XenotypeDef xenotypeDef = def as XenotypeDef;
				if (xenotypeDef != null)
				{
					GUI.color = (color ?? XenotypeDef.IconColor);
					Widgets.DrawTextureFitted(rect, xenotypeDef.Icon, scale, material, alpha);
					GUI.color = Color.white;
					return;
				}
				PsychicRitualDef psychicRitualDef = def as PsychicRitualDef;
				if (psychicRitualDef != null)
				{
					Widgets.DrawTextureFitted(rect, psychicRitualDef.uiIcon, scale, material, alpha);
					return;
				}
				if (drawPlaceholder)
				{
					Widgets.DrawTextureFitted(rect, Widgets.PlaceholderIconTex, scale, material, alpha);
				}
			}
		}

		// Token: 0x0600330A RID: 13066 RVA: 0x00146900 File Offset: 0x00144B00
		public static void ThingIcon(Rect rect, Thing thing, float alpha = 1f, Rot4? rot = null, bool stackOfOne = false, float scale = 1f, bool grayscale = false)
		{
			thing = thing.GetInnerIfMinified();
			Blueprint blueprint = thing as Blueprint;
			if (blueprint != null && blueprint.EntityToBuild() != null)
			{
				Widgets.DefIcon(rect, blueprint.EntityToBuild(), blueprint.EntityToBuildStuff(), 1f, blueprint.EntityToBuildStyle(), false, null, null, null, alpha);
				return;
			}
			float num;
			float resolvedIconAngle;
			Vector2 vector;
			Color color;
			Material material;
			Texture iconFor = Widgets.GetIconFor(thing, new Vector2(rect.width, rect.height), rot, stackOfOne, out num, out resolvedIconAngle, out vector, out color, out material);
			Frame frame = thing as Frame;
			if (frame != null && frame.BuildDef != null)
			{
				iconFor = Widgets.GetIconFor(frame.BuildDef, frame.Stuff, frame.StyleDef, null);
			}
			if (iconFor == null || iconFor == BaseContent.BadTex)
			{
				return;
			}
			GUI.color = color;
			ThingStyleDef styleDef = thing.StyleDef;
			if ((styleDef != null && styleDef.UIIcon != null) || !thing.def.uiIconPath.NullOrEmpty())
			{
				rect.position += new Vector2(thing.def.uiIconOffset.x * rect.size.x, thing.def.uiIconOffset.y * rect.size.y);
			}
			Material mat = material;
			if (grayscale)
			{
				MaterialRequest req = new MaterialRequest
				{
					shader = ShaderDatabase.GrayscaleGUI,
					color = color
				};
				if (material != null)
				{
					req.maskTex = (Texture2D)material.GetTexture(ShaderPropertyIDs.MaskTex);
					req.color = material.GetColor(ShaderPropertyIDs.Color);
					req.colorTwo = material.GetColor(ShaderPropertyIDs.ColorTwo);
				}
				else
				{
					req.maskTex = Texture2D.redTexture;
				}
				mat = MaterialPool.MatFrom(req);
			}
			Widgets.ThingIconWorker(rect, thing.def, iconFor, resolvedIconAngle, num * scale, rot, mat, alpha);
			GUI.color = Color.white;
		}

		// Token: 0x0600330B RID: 13067 RVA: 0x00146B0C File Offset: 0x00144D0C
		public static void ThingIcon(Rect rect, ThingDef thingDef, ThingDef stuffDef = null, ThingStyleDef thingStyleDef = null, float scale = 1f, Color? color = null, int? graphicIndexOverride = null, float alpha = 1f)
		{
			if (thingDef.uiIcon == null || thingDef.uiIcon == BaseContent.BadTex)
			{
				return;
			}
			Material material;
			Texture2D iconFor = Widgets.GetIconFor(thingDef, out material, stuffDef, thingStyleDef, graphicIndexOverride);
			if (iconFor == null)
			{
				return;
			}
			Color color2 = GUI.color;
			if (color != null)
			{
				GUI.color = color.Value;
			}
			else if (stuffDef != null)
			{
				GUI.color = thingDef.GetColorForStuff(stuffDef);
			}
			else if (material != null)
			{
				GUI.color = Color.white;
			}
			else
			{
				GUI.color = (thingDef.MadeFromStuff ? thingDef.GetColorForStuff(GenStuff.DefaultStuffFor(thingDef)) : thingDef.uiIconColor);
			}
			if (thingStyleDef != null)
			{
				scale *= thingStyleDef.uiIconScale;
			}
			else
			{
				scale *= GenUI.IconDrawScale(thingDef);
			}
			float num = (float)iconFor.width / (float)iconFor.height;
			rect = ((num < 1f) ? rect.MiddlePart(num, 1f) : rect.MiddlePart(1f, num));
			Rect rect2 = rect;
			Texture resolvedIcon = iconFor;
			float uiIconAngle = thingDef.uiIconAngle;
			float scale2 = scale;
			Material mat = material;
			Widgets.ThingIconWorker(rect2, thingDef, resolvedIcon, uiIconAngle, scale2, null, mat, alpha);
			GUI.color = color2;
		}

		// Token: 0x0600330C RID: 13068 RVA: 0x00146C2C File Offset: 0x00144E2C
		public static Texture2D GetIconFor(ThingDef thingDef, ThingDef stuffDef = null, ThingStyleDef thingStyleDef = null, int? graphicIndexOverride = null)
		{
			Material material;
			return Widgets.GetIconFor(thingDef, out material, stuffDef, thingStyleDef, graphicIndexOverride);
		}

		// Token: 0x0600330D RID: 13069 RVA: 0x00146C44 File Offset: 0x00144E44
		public static Texture2D GetIconFor(ThingDef thingDef, out Material material, ThingDef stuffDef = null, ThingStyleDef thingStyleDef = null, int? graphicIndexOverride = null)
		{
			if (thingDef.IsCorpse)
			{
				IngestibleProperties ingestible = thingDef.ingestible;
				if (((ingestible != null) ? ingestible.sourceDef : null) != null)
				{
					thingDef = thingDef.ingestible.sourceDef;
				}
			}
			material = null;
			Texture2D result = thingDef.GetUIIconForStuff(stuffDef);
			if (thingStyleDef != null && thingStyleDef.UIIcon != null)
			{
				if (graphicIndexOverride != null)
				{
					result = thingStyleDef.IconForIndex(graphicIndexOverride.Value, null);
				}
				else
				{
					result = thingStyleDef.UIIcon;
				}
			}
			else
			{
				Graphic_Appearances graphic_Appearances = thingDef.graphic as Graphic_Appearances;
				if (graphic_Appearances != null)
				{
					result = (Texture2D)graphic_Appearances.SubGraphicFor(stuffDef ?? GenStuff.DefaultStuffFor(thingDef)).MatAt(thingDef.defaultPlacingRot, null).mainTexture;
				}
				else if (thingDef.uiIconMaterial != null)
				{
					material = thingDef.uiIconMaterial;
				}
			}
			return result;
		}

		// Token: 0x0600330E RID: 13070 RVA: 0x00146D14 File Offset: 0x00144F14
		private static Color GetDrawColor(BuildableDef buildable, ThingDef stuff)
		{
			ThingDef thingDef = buildable as ThingDef;
			if (thingDef != null)
			{
				if (stuff != null)
				{
					return thingDef.GetColorForStuff(stuff);
				}
				if (thingDef.graphicData != null)
				{
					return thingDef.graphicData.color;
				}
			}
			else
			{
				TerrainDef terrainDef = buildable as TerrainDef;
				if (terrainDef != null)
				{
					return terrainDef.DrawColor;
				}
			}
			return Color.white;
		}

		// Token: 0x0600330F RID: 13071 RVA: 0x00146D60 File Offset: 0x00144F60
		public static Texture GetIconFor(Thing thing, Vector2 size, Rot4? rot, bool stackOfOne, out float scale, out float angle, out Vector2 iconProportions, out Color color, out Material material)
		{
			if (thing == null)
			{
				scale = 1f;
				angle = 0f;
				iconProportions = Vector2.one;
				color = Color.white;
				material = null;
				return null;
			}
			material = null;
			thing = thing.GetInnerIfMinified();
			Corpse corpse = thing as Corpse;
			if (corpse != null)
			{
				thing = corpse.InnerPawn;
			}
			Texture result = null;
			ThingStyleDef styleDef = thing.StyleDef;
			iconProportions = thing.DrawSize;
			color = thing.DrawColor;
			scale = GenUI.IconDrawScale(thing.def);
			Blueprint blueprint = thing as Blueprint;
			if (blueprint != null)
			{
				color = Widgets.GetDrawColor(blueprint.EntityToBuild(), blueprint.EntityToBuildStuff());
			}
			if (rot != null && rot.Value.IsHorizontal)
			{
				iconProportions = new Vector2(iconProportions.y, iconProportions.x);
			}
			angle = 0f;
			if (thing.UIIconOverride != null)
			{
				result = thing.UIIconOverride;
				angle = thing.def.uiIconAngle;
			}
			else if (styleDef != null && styleDef.UIIcon != null)
			{
				Rot4 value = rot.GetValueOrDefault();
				if (rot == null)
				{
					value = thing.def.defaultPlacingRot;
					rot = new Rot4?(value);
				}
				result = styleDef.IconForIndex(thing.OverrideGraphicIndex ?? thing.thingIDNumber, rot);
				angle = thing.def.uiIconAngle;
			}
			else if (!thing.def.uiIconPath.NullOrEmpty())
			{
				result = thing.def.uiIcon;
				angle = thing.def.uiIconAngle;
			}
			else
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null)
				{
					if (!pawn.RaceProps.Humanlike)
					{
						Rot4 value = rot.GetValueOrDefault();
						if (rot == null)
						{
							value = Rot4.East;
							rot = new Rot4?(value);
						}
						Pawn_DrawTracker drawer = pawn.Drawer;
						if (drawer != null)
						{
							PawnRenderer renderer = drawer.renderer;
							if (renderer != null)
							{
								renderer.EnsureGraphicsInitialized();
							}
						}
						Pawn_DrawTracker drawer2 = pawn.Drawer;
						Material material2;
						if (drawer2 == null)
						{
							material2 = null;
						}
						else
						{
							PawnRenderer renderer2 = drawer2.renderer;
							if (renderer2 == null)
							{
								material2 = null;
							}
							else
							{
								Graphic bodyGraphic = renderer2.BodyGraphic;
								material2 = ((bodyGraphic != null) ? bodyGraphic.MatAt(rot.Value, null) : null);
							}
						}
						Material material3 = material2;
						if (material3 != null)
						{
							result = material3.mainTexture;
							Shader shader;
							MaterialRequest req;
							if (ShaderDatabase.TryGetUIShader(material3.shader, out shader) && MaterialPool.TryGetRequestForMat(material3, out req))
							{
								req.shader = shader;
								material = MaterialPool.MatFrom(req);
								color = Color.white;
							}
							else
							{
								color = material3.color;
							}
						}
					}
					else
					{
						Rot4 value = rot.GetValueOrDefault();
						if (rot == null)
						{
							value = Rot4.South;
							rot = new Rot4?(value);
						}
						Rect r = new Rect(0f, 0f, size.x, size.y).ScaledBy(1.8f);
						r = r.Rounded();
						float num = 1.8f;
						ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
						if (ChildcareUtility.CanSuckle(pawn, out breastfeedFailReason))
						{
							num = 3f;
						}
						else
						{
							r.y += 3f;
						}
						Pawn pawn2 = pawn;
						Vector2 size2 = new Vector2(r.width, r.height);
						Rot4 value2 = rot.Value;
						float cameraZoom = num;
						result = PortraitsCache.Get(pawn2, size2, value2, default(Vector3), cameraZoom, true, true, true, true, null, null, false, null);
					}
				}
				else
				{
					Rot4 value = rot.GetValueOrDefault();
					if (rot == null)
					{
						value = thing.def.defaultPlacingRot;
						rot = new Rot4?(value);
					}
					Material material4;
					if (stackOfOne)
					{
						Graphic_StackCount graphic_StackCount = thing.Graphic as Graphic_StackCount;
						if (graphic_StackCount != null && !(thing.Graphic is Graphic_MealVariants))
						{
							material4 = graphic_StackCount.SubGraphicForStackCount(1, thing.def).MatSingleFor(thing);
						}
						else
						{
							material4 = thing.Graphic.ExtractInnerGraphicFor(thing, null).MatAt(rot.Value, null);
						}
					}
					else
					{
						Graphic_Linked graphic_Linked = thing.Graphic as Graphic_Linked;
						if (graphic_Linked != null)
						{
							material4 = graphic_Linked.SubGraphic.ExtractInnerGraphicFor(thing, null).MatAt(rot.Value, null);
						}
						else
						{
							material4 = thing.Graphic.ExtractInnerGraphicFor(thing, null).MatAt(rot.Value, null);
						}
					}
					result = material4.mainTexture;
					Shader shader2;
					MaterialRequest req2;
					if (ShaderDatabase.TryGetUIShader(material4.shader, out shader2) && MaterialPool.TryGetRequestForMat(material4, out req2))
					{
						req2.shader = shader2;
						material = MaterialPool.MatFrom(req2);
						color = Color.white;
					}
				}
			}
			return result;
		}

		// Token: 0x06003310 RID: 13072 RVA: 0x00147214 File Offset: 0x00145414
		private static void ThingIconWorker(Rect rect, ThingDef thingDef, Texture resolvedIcon, float resolvedIconAngle, float scale = 1f, Rot4? rot = null, Material mat = null, float alpha = 1f)
		{
			Vector2 texProportions = new Vector2((float)resolvedIcon.width, (float)resolvedIcon.height);
			Rect texCoords = Widgets.DefaultTexCoords;
			Rot4 value = rot.GetValueOrDefault();
			if (rot == null)
			{
				value = thingDef.defaultPlacingRot;
				rot = new Rot4?(value);
			}
			if (thingDef.graphicData != null)
			{
				texProportions = (rot.Value.IsHorizontal ? thingDef.graphicData.drawSize.Rotated() : thingDef.graphicData.drawSize);
				if (thingDef.uiIconPath.NullOrEmpty() && thingDef.graphicData.linkFlags != LinkFlags.None)
				{
					texCoords = Widgets.LinkedTexCoords;
				}
			}
			Widgets.DrawTextureFitted(rect, resolvedIcon, scale, texProportions, texCoords, resolvedIconAngle, mat, alpha);
		}

		// Token: 0x06003311 RID: 13073 RVA: 0x001472C3 File Offset: 0x001454C3
		public static Rect CroppedTerrainTextureRect(Texture2D tex)
		{
			return new Rect(0f, 0f, 64f / (float)tex.width, 64f / (float)tex.height);
		}

		// Token: 0x06003312 RID: 13074 RVA: 0x001472EE File Offset: 0x001454EE
		public static void DrawAltRect(Rect rect)
		{
			GUI.DrawTexture(rect, Widgets.AltTexture);
		}

		// Token: 0x06003313 RID: 13075 RVA: 0x001472FC File Offset: 0x001454FC
		public static void ListSeparator(ref RectDivider divider, string label)
		{
			RectDivider rect = divider.NewRow(25f, VerticalJustification.Top, null);
			GUI.BeginGroup(rect);
			float num = 0f;
			Widgets.ListSeparator(ref num, rect.Rect.width, label);
			GUI.EndGroup();
		}

		// Token: 0x06003314 RID: 13076 RVA: 0x0014734C File Offset: 0x0014554C
		public static void ListSeparator(ref float curY, float width, string label)
		{
			Color color = GUI.color;
			curY += 3f;
			GUI.color = Widgets.SeparatorLabelColor;
			Rect rect = new Rect(0f, curY, width, 30f);
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.Label(rect, label);
			curY += 20f;
			GUI.color = Widgets.SeparatorLineColor;
			Widgets.DrawLineHorizontal(0f, curY, width);
			curY += 2f;
			GUI.color = color;
		}

		// Token: 0x06003315 RID: 13077 RVA: 0x001473C0 File Offset: 0x001455C0
		public static void DrawLine(Vector2 start, Vector2 end, Color color, float width)
		{
			float num = end.x - start.x;
			float num2 = end.y - start.y;
			float num3 = Mathf.Sqrt(num * num + num2 * num2);
			if (num3 < 0.01f)
			{
				return;
			}
			width *= 3f;
			float num4 = width * num2 / num3;
			float num5 = width * num / num3;
			float z = -Mathf.Atan2(-num2, num) * 57.29578f;
			Vector2 vector = start + new Vector2(0.5f * num4, -0.5f * num5);
			Matrix4x4 m = Matrix4x4.TRS(vector, Quaternion.Euler(0f, 0f, z), Vector3.one) * Matrix4x4.TRS(-vector, Quaternion.identity, Vector3.one);
			Rect position = new Rect(start.x, start.y - 0.5f * num5, num3, width);
			GL.PushMatrix();
			GL.MultMatrix(m);
			GUI.DrawTexture(position, Widgets.LineTexAA, ScaleMode.StretchToFill, true, 0f, color, 0f, 0f);
			GL.PopMatrix();
		}

		// Token: 0x06003316 RID: 13078 RVA: 0x001474CF File Offset: 0x001456CF
		public static void DrawLineHorizontal(float x, float y, float length, Color color)
		{
			Widgets.DrawBoxSolid(new Rect(x, y, length, 1f), color);
		}

		// Token: 0x06003317 RID: 13079 RVA: 0x001474E4 File Offset: 0x001456E4
		public static void DrawLineHorizontal(float x, float y, float length)
		{
			GUI.DrawTexture(new Rect(x, y, length, 1f), BaseContent.WhiteTex);
		}

		// Token: 0x06003318 RID: 13080 RVA: 0x001474FD File Offset: 0x001456FD
		public static void DrawLineVertical(float x, float y, float length)
		{
			GUI.DrawTexture(new Rect(x, y, 1f, length), BaseContent.WhiteTex);
		}

		// Token: 0x06003319 RID: 13081 RVA: 0x00147516 File Offset: 0x00145716
		public static void DrawBoxSolid(Rect rect, Color color)
		{
			Color color2 = GUI.color;
			GUI.color = color;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = color2;
		}

		// Token: 0x0600331A RID: 13082 RVA: 0x00147533 File Offset: 0x00145733
		public static void DrawBoxSolidWithOutline(Rect rect, Color solidColor, Color outlineColor, int outlineThickness = 1)
		{
			Widgets.DrawBoxSolid(rect, solidColor);
			Color color = GUI.color;
			GUI.color = outlineColor;
			Widgets.DrawBox(rect, outlineThickness, null);
			GUI.color = color;
		}

		// Token: 0x0600331B RID: 13083 RVA: 0x00147554 File Offset: 0x00145754
		public static void DrawBox(Rect rect, int thickness = 1, Texture2D lineTexture = null)
		{
			Vector2 vector = new Vector2(rect.x, rect.y);
			Vector2 vector2 = new Vector2(rect.x + rect.width, rect.y + rect.height);
			if (vector.x > vector2.x)
			{
				ref float ptr = ref vector.x;
				float num = vector2.x;
				float num2 = vector.x;
				ptr = num;
				vector2.x = num2;
			}
			if (vector.y > vector2.y)
			{
				ref float ptr = ref vector.y;
				float num2 = vector2.y;
				float num = vector.y;
				ptr = num2;
				vector2.y = num;
			}
			Vector3 vector3 = vector2 - vector;
			GUI.DrawTexture(UIScaling.AdjustRectToUIScaling(new Rect(vector.x, vector.y, (float)thickness, vector3.y)), lineTexture ?? BaseContent.WhiteTex);
			GUI.DrawTexture(UIScaling.AdjustRectToUIScaling(new Rect(vector2.x - (float)thickness, vector.y, (float)thickness, vector3.y)), lineTexture ?? BaseContent.WhiteTex);
			GUI.DrawTexture(UIScaling.AdjustRectToUIScaling(new Rect(vector.x + (float)thickness, vector.y, vector3.x - (float)(thickness * 2), (float)thickness)), lineTexture ?? BaseContent.WhiteTex);
			GUI.DrawTexture(UIScaling.AdjustRectToUIScaling(new Rect(vector.x + (float)thickness, vector2.y - (float)thickness, vector3.x - (float)(thickness * 2), (float)thickness)), lineTexture ?? BaseContent.WhiteTex);
		}

		// Token: 0x0600331C RID: 13084 RVA: 0x001476DC File Offset: 0x001458DC
		public static void LabelCacheHeight(ref Rect rect, string label, bool renderLabel = true, bool forceInvalidation = false)
		{
			bool flag = Widgets.LabelCache.ContainsKey(label);
			if (forceInvalidation)
			{
				flag = false;
			}
			float height;
			if (flag)
			{
				height = Widgets.LabelCache[label];
			}
			else
			{
				height = Text.CalcHeight(label, rect.width);
			}
			rect.height = height;
			if (renderLabel)
			{
				Widgets.Label(rect, label);
			}
		}

		// Token: 0x0600331D RID: 13085 RVA: 0x00147734 File Offset: 0x00145934
		public static void Label(Rect rect, GUIContent content)
		{
			GUI.Label(rect, content, Text.CurFontStyle);
		}

		// Token: 0x0600331E RID: 13086 RVA: 0x00147742 File Offset: 0x00145942
		public static void LabelEllipses(Rect rect, string label)
		{
			label = Text.ClampTextWithEllipsis(rect, label);
			Widgets.Label(rect, label);
		}

		// Token: 0x0600331F RID: 13087 RVA: 0x00147754 File Offset: 0x00145954
		public static void Label(Rect rect, string label)
		{
			GUI.Label(rect, label, Text.CurFontStyle);
		}

		// Token: 0x06003320 RID: 13088 RVA: 0x00147762 File Offset: 0x00145962
		public static void Label(Rect rect, TaggedString label)
		{
			Widgets.Label(rect, label.Resolve());
		}

		// Token: 0x06003321 RID: 13089 RVA: 0x00147774 File Offset: 0x00145974
		public static void Label(float x, ref float curY, float width, string text, TipSignal tip = default(TipSignal))
		{
			if (text.NullOrEmpty())
			{
				return;
			}
			float num = Text.CalcHeight(text, width);
			Rect rect = new Rect(x, curY, width, num);
			if (!tip.text.NullOrEmpty() || tip.textGetter != null)
			{
				float x2 = Text.CalcSize(text).x;
				Rect rect2 = new Rect(rect.x, rect.y, x2, num);
				Widgets.DrawHighlightIfMouseover(rect2);
				TooltipHandler.TipRegion(rect2, tip);
			}
			Widgets.Label(rect, text);
			curY += num;
		}

		// Token: 0x06003322 RID: 13090 RVA: 0x001477F0 File Offset: 0x001459F0
		public static void Label(Rect rect, ref float y, string text, TipSignal tip = default(TipSignal))
		{
			if (text.NullOrEmpty())
			{
				return;
			}
			Widgets.Label(rect.x, ref y, rect.width, text, tip);
		}

		// Token: 0x06003323 RID: 13091 RVA: 0x00147814 File Offset: 0x00145A14
		public static void LongLabel(float x, float width, string label, ref float curY, bool draw = true)
		{
			if (label.Length < 2500)
			{
				if (draw)
				{
					Widgets.Label(new Rect(x, curY, width, 1000f), label);
				}
				curY += Text.CalcHeight(label, width);
				return;
			}
			int num = 0;
			int num2 = -1;
			bool flag = false;
			for (int i = 0; i < label.Length; i++)
			{
				if (label[i] == '\n')
				{
					num++;
					if (num >= 50)
					{
						string text = label.Substring(num2 + 1, i - num2 - 1);
						num2 = i;
						num = 0;
						if (flag)
						{
							curY += Text.SpaceBetweenLines;
						}
						if (draw)
						{
							Widgets.Label(new Rect(x, curY, width, 10000f), text);
						}
						curY += Text.CalcHeight(text, width);
						flag = true;
					}
				}
			}
			if (num2 != label.Length - 1)
			{
				if (flag)
				{
					curY += Text.SpaceBetweenLines;
				}
				string text2 = label.Substring(num2 + 1);
				if (draw)
				{
					Widgets.Label(new Rect(x, curY, width, 10000f), text2);
				}
				curY += Text.CalcHeight(text2, width);
			}
		}

		// Token: 0x06003324 RID: 13092 RVA: 0x00147914 File Offset: 0x00145B14
		public static void LabelScrollable(Rect rect, string label, ref Vector2 scrollbarPosition, bool dontConsumeScrollEventsIfNoScrollbar = false, bool takeScrollbarSpaceEvenIfNoScrollbar = true, bool longLabel = false)
		{
			bool flag = takeScrollbarSpaceEvenIfNoScrollbar || Text.CalcHeight(label, rect.width) > rect.height;
			bool flag2 = flag && (!dontConsumeScrollEventsIfNoScrollbar || Text.CalcHeight(label, rect.width - 16f) > rect.height);
			float num = rect.width;
			if (flag)
			{
				num -= 16f;
			}
			float num2;
			if (longLabel)
			{
				num2 = 0f;
				Widgets.LongLabel(0f, num, label, ref num2, false);
			}
			else
			{
				num2 = Text.CalcHeight(label, num);
			}
			Rect rect2 = new Rect(0f, 0f, num, Mathf.Max(num2 + 5f, rect.height));
			if (flag2)
			{
				Widgets.BeginScrollView(rect, ref scrollbarPosition, rect2, true);
			}
			else
			{
				Widgets.BeginGroup(rect);
			}
			if (longLabel)
			{
				float y = rect2.y;
				Widgets.LongLabel(rect2.x, rect2.width, label, ref y, true);
			}
			else
			{
				Widgets.Label(rect2, label);
			}
			if (flag2)
			{
				Widgets.EndScrollView();
				return;
			}
			Widgets.EndGroup();
		}

		// Token: 0x06003325 RID: 13093 RVA: 0x00147A10 File Offset: 0x00145C10
		public static void LabelWithIcon(Rect rect, string label, Texture2D labelIcon, float labelIconScale = 1f)
		{
			float num = Mathf.Min((float)labelIcon.width, rect.height);
			Rect outerRect = new Rect(rect.x, rect.y, num, rect.height);
			rect.xMin += num;
			Widgets.DrawTextureFitted(outerRect, labelIcon, labelIconScale, 1f);
			Widgets.Label(rect, label);
		}

		// Token: 0x06003326 RID: 13094 RVA: 0x00147A70 File Offset: 0x00145C70
		public static void DefLabelWithIcon(Rect rect, Def def, float iconMargin = 2f, float textOffsetX = 6f)
		{
			Widgets.DrawHighlightIfMouseover(rect);
			TooltipHandler.TipRegion(rect, def.description);
			Widgets.BeginGroup(rect);
			Rect rect2 = new Rect(0f, 0f, rect.height, rect.height);
			if (iconMargin != 0f)
			{
				rect2 = rect2.ContractedBy(iconMargin);
			}
			Widgets.DefIcon(rect2, def, null, 1f, null, true, null, null, null, 1f);
			Rect rect3 = new Rect(rect2.xMax + textOffsetX, 0f, rect.width, rect.height);
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Widgets.Label(rect3, def.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.WordWrap = true;
			Widgets.EndGroup();
		}

		// Token: 0x06003327 RID: 13095 RVA: 0x00147B3C File Offset: 0x00145D3C
		public static bool LabelFit(Rect rect, string label)
		{
			bool result = false;
			GameFont font = Text.Font;
			Text.Font = GameFont.Small;
			if (Text.CalcSize(label).x <= rect.width)
			{
				Widgets.Label(rect, label);
			}
			else
			{
				Text.Font = GameFont.Tiny;
				if (Text.CalcSize(label).x <= rect.width)
				{
					Widgets.Label(rect, label);
				}
				else
				{
					Widgets.LabelEllipses(rect, label);
					result = true;
				}
				Text.Font = GameFont.Small;
			}
			Text.Font = font;
			return result;
		}

		// Token: 0x06003328 RID: 13096 RVA: 0x00147BAC File Offset: 0x00145DAC
		public static void HyperlinkWithIcon(Rect rect, Dialog_InfoCard.Hyperlink hyperlink, string text = null, float iconMargin = 2f, float textOffsetX = 6f, Color? color = null, bool truncateLabel = false, string textSuffix = null)
		{
			string text2 = text ?? hyperlink.Label.CapitalizeFirst();
			if (textSuffix != null)
			{
				text2 += textSuffix;
			}
			Widgets.BeginGroup(rect);
			Rect rect2 = new Rect(0f, 0f, rect.height, rect.height);
			if (iconMargin != 0f)
			{
				rect2 = rect2.ContractedBy(iconMargin);
			}
			if (hyperlink.IsHidden)
			{
				Widgets.DrawTextureFitted(rect2, Widgets.PlaceholderIconTex, 1f, 1f);
			}
			else if (hyperlink.thing != null)
			{
				Widgets.ThingIcon(rect2, hyperlink.thing, 1f, null, false, 1f, false);
			}
			else
			{
				Widgets.DefIcon(rect2, hyperlink.def, null, 1f, null, true, null, null, null, 1f);
			}
			float num = rect2.xMax + textOffsetX;
			Rect rect3 = new Rect(rect2.xMax + textOffsetX, 0f, rect.width - num, rect.height);
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Color textColor = color ?? Widgets.NormalOptionColor;
			if (hyperlink.IsHidden)
			{
				textColor = Color.gray;
			}
			Widgets.ButtonText(rect3, truncateLabel ? text2.Truncate(rect3.width, null) : text2, false, false, textColor, false, null);
			if (Widgets.ButtonInvisible(rect3, true))
			{
				hyperlink.ActivateHyperlink();
			}
			Text.Anchor = TextAnchor.UpperLeft;
			Text.WordWrap = true;
			Widgets.EndGroup();
		}

		// Token: 0x06003329 RID: 13097 RVA: 0x00147D40 File Offset: 0x00145F40
		public static void DrawNumberOnMap(Vector2 screenPos, int number, Color textColor)
		{
			Text.Anchor = TextAnchor.MiddleCenter;
			Text.Font = GameFont.Medium;
			string text = number.ToStringCached();
			float val = Text.CalcSize(text).x + 8f;
			Rect rect = new Rect(screenPos.x - 20f, screenPos.y - 15f, Math.Max(40f, val), 30f);
			GUI.DrawTexture(rect, TexUI.GrayBg);
			GUI.color = textColor;
			Widgets.Label(rect, text);
			GUI.color = Color.white;
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x0600332A RID: 13098 RVA: 0x00147DCC File Offset: 0x00145FCC
		public static void DrawStringOnMap(Vector2 screenPos, string str, Color textColor)
		{
			Text.Anchor = TextAnchor.MiddleCenter;
			Text.Font = GameFont.Medium;
			float num = Text.CalcSize(str).x + 8f;
			Rect rect = new Rect(screenPos.x - num / 2f, screenPos.y - 15f, num, 30f);
			GUI.DrawTexture(rect, TexUI.GrayBg);
			GUI.color = textColor;
			Widgets.Label(rect, str);
			GUI.color = Color.white;
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x0600332B RID: 13099 RVA: 0x00147E49 File Offset: 0x00146049
		public static void Checkbox(Vector2 topLeft, ref bool checkOn, float size = 24f, bool disabled = false, bool paintable = false, Texture2D texChecked = null, Texture2D texUnchecked = null)
		{
			Widgets.Checkbox(topLeft.x, topLeft.y, ref checkOn, size, disabled, paintable, texChecked, texUnchecked);
		}

		// Token: 0x0600332C RID: 13100 RVA: 0x00147E68 File Offset: 0x00146068
		public static void Checkbox(float x, float y, ref bool checkOn, float size = 24f, bool disabled = false, bool paintable = false, Texture2D texChecked = null, Texture2D texUnchecked = null)
		{
			if (disabled)
			{
				GUI.color = Widgets.InactiveColor;
			}
			Rect rect = new Rect(x, y, size, size);
			Widgets.CheckboxDraw(x, y, checkOn, disabled, size, texChecked, texUnchecked);
			if (!disabled)
			{
				Widgets.ToggleInvisibleDraggable(rect, ref checkOn, true, paintable);
			}
			if (disabled)
			{
				GUI.color = Color.white;
			}
		}

		// Token: 0x0600332D RID: 13101 RVA: 0x00147EBC File Offset: 0x001460BC
		public static void CheckboxLabeled(Rect rect, string label, ref bool checkOn, bool disabled = false, Texture2D texChecked = null, Texture2D texUnchecked = null, bool placeCheckboxNearText = false, bool paintable = false)
		{
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleLeft;
			if (placeCheckboxNearText)
			{
				rect.width = Mathf.Min(rect.width, Text.CalcSize(label).x + 24f + 10f);
			}
			Rect rect2 = rect;
			rect2.xMax -= 24f;
			Widgets.Label(rect2, label);
			if (!disabled)
			{
				Widgets.ToggleInvisibleDraggable(rect, ref checkOn, true, paintable);
			}
			Widgets.CheckboxDraw(rect.x + rect.width - 24f, rect.y + (rect.height - 24f) / 2f, checkOn, disabled, 24f, texChecked, texUnchecked);
			Text.Anchor = anchor;
		}

		// Token: 0x0600332E RID: 13102 RVA: 0x00147F74 File Offset: 0x00146174
		public static void ToggleInvisibleDraggable(Rect rect, ref bool checkOn, bool doMouseoverSound = false, bool paintable = false)
		{
			Widgets.DraggableResult draggableResult = Widgets.ButtonInvisibleDraggable(rect, doMouseoverSound);
			bool flag = false;
			if (draggableResult == Widgets.DraggableResult.Pressed)
			{
				checkOn = !checkOn;
				flag = true;
			}
			else if (draggableResult == Widgets.DraggableResult.Dragged && paintable)
			{
				checkOn = !checkOn;
				flag = true;
				Widgets.checkboxPainting = true;
				Widgets.checkboxPaintingState = checkOn;
			}
			if (paintable && Mouse.IsOver(rect) && Widgets.checkboxPainting && Input.GetMouseButton(0) && checkOn != Widgets.checkboxPaintingState)
			{
				checkOn = Widgets.checkboxPaintingState;
				flag = true;
			}
			if (doMouseoverSound && flag)
			{
				if (checkOn)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
					return;
				}
				SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x0600332F RID: 13103 RVA: 0x00148004 File Offset: 0x00146204
		public static bool CheckboxLabeledSelectable(Rect rect, string label, ref bool selected, ref bool checkOn, Texture2D labelIcon = null, float labelIconScale = 1f)
		{
			if (selected)
			{
				Widgets.DrawHighlight(rect);
			}
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleLeft;
			if (labelIcon != null)
			{
				Rect outerRect = new Rect(rect.x, rect.y, (float)labelIcon.width, rect.height);
				rect.xMin += (float)labelIcon.width;
				Widgets.DrawTextureFitted(outerRect, labelIcon, labelIconScale, 1f);
			}
			Widgets.Label(rect, label);
			Text.Anchor = anchor;
			bool flag = selected;
			Rect butRect = rect;
			butRect.width -= 24f;
			if (!selected && Widgets.ButtonInvisible(butRect, true))
			{
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
				selected = true;
			}
			Color color = GUI.color;
			GUI.color = Color.white;
			Widgets.CheckboxDraw(rect.xMax - 24f, rect.y, checkOn, false, 24f, null, null);
			GUI.color = color;
			if (Widgets.ButtonInvisible(new Rect(rect.xMax - 24f, rect.y, 24f, 24f), true))
			{
				checkOn = !checkOn;
				if (checkOn)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
			}
			return selected && !flag;
		}

		// Token: 0x06003330 RID: 13104 RVA: 0x00148144 File Offset: 0x00146344
		public static Texture2D GetCheckboxTexture(bool state)
		{
			if (state)
			{
				return Widgets.CheckboxOnTex;
			}
			return Widgets.CheckboxOffTex;
		}

		// Token: 0x06003331 RID: 13105 RVA: 0x00148154 File Offset: 0x00146354
		public static void CheckboxDraw(float x, float y, bool active, bool disabled, float size = 24f, Texture2D texChecked = null, Texture2D texUnchecked = null)
		{
			Color color = GUI.color;
			if (disabled)
			{
				GUI.color = Widgets.InactiveColor;
			}
			Texture2D image;
			if (active)
			{
				image = ((texChecked != null) ? texChecked : Widgets.CheckboxOnTex);
			}
			else
			{
				image = ((texUnchecked != null) ? texUnchecked : Widgets.CheckboxOffTex);
			}
			GUI.DrawTexture(new Rect(x, y, size, size), image);
			if (disabled)
			{
				GUI.color = color;
			}
		}

		// Token: 0x06003332 RID: 13106 RVA: 0x001481BC File Offset: 0x001463BC
		public static MultiCheckboxState CheckboxMulti(Rect rect, MultiCheckboxState state, bool paintable = false)
		{
			Texture2D tex;
			if (state == MultiCheckboxState.On)
			{
				tex = Widgets.CheckboxOnTex;
			}
			else if (state == MultiCheckboxState.Off)
			{
				tex = Widgets.CheckboxOffTex;
			}
			else
			{
				tex = Widgets.CheckboxPartialTex;
			}
			MouseoverSounds.DoRegion(rect);
			MultiCheckboxState multiCheckboxState = (state == MultiCheckboxState.Off) ? MultiCheckboxState.On : MultiCheckboxState.Off;
			bool flag = false;
			Widgets.DraggableResult draggableResult = Widgets.ButtonImageDraggable(rect, tex);
			if (paintable && draggableResult == Widgets.DraggableResult.Dragged)
			{
				Widgets.checkboxPainting = true;
				Widgets.checkboxPaintingState = (multiCheckboxState == MultiCheckboxState.On);
				flag = true;
			}
			else if (draggableResult.AnyPressed())
			{
				flag = true;
			}
			else if (paintable && Widgets.checkboxPainting && Mouse.IsOver(rect))
			{
				multiCheckboxState = (Widgets.checkboxPaintingState ? MultiCheckboxState.On : MultiCheckboxState.Off);
				if (state != multiCheckboxState)
				{
					flag = true;
				}
			}
			if (flag)
			{
				if (multiCheckboxState == MultiCheckboxState.On)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
				return multiCheckboxState;
			}
			return state;
		}

		// Token: 0x06003333 RID: 13107 RVA: 0x0014826A File Offset: 0x0014646A
		public static bool RadioButton(Vector2 topLeft, bool chosen, bool disabled = false)
		{
			return Widgets.RadioButton(topLeft.x, topLeft.y, chosen, disabled);
		}

		// Token: 0x06003334 RID: 13108 RVA: 0x0014827F File Offset: 0x0014647F
		public static bool RadioButton(float x, float y, bool chosen, bool disabled = false)
		{
			Rect butRect = new Rect(x, y, 24f, 24f);
			Widgets.RadioButtonDraw(x, y, chosen, disabled);
			bool flag = Widgets.ButtonInvisible(butRect, true);
			if (flag && !chosen)
			{
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			}
			return flag;
		}

		// Token: 0x06003335 RID: 13109 RVA: 0x001482B4 File Offset: 0x001464B4
		public static bool RadioButtonLabeled(Rect rect, string labelText, bool chosen, bool disabled = false)
		{
			using (new TextBlock(TextAnchor.MiddleLeft, disabled ? ColoredText.SubtleGrayColor : Color.white))
			{
				Widgets.Label(rect, labelText);
			}
			bool flag = Widgets.ButtonInvisible(rect, true);
			if (flag && !chosen && !disabled)
			{
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			}
			Widgets.RadioButtonDraw(rect.x + rect.width - 24f, rect.y + rect.height / 2f - 12f, chosen, disabled);
			return flag;
		}

		// Token: 0x06003336 RID: 13110 RVA: 0x00148354 File Offset: 0x00146554
		private static void RadioButtonDraw(float x, float y, bool chosen, bool disabled)
		{
			Color color = GUI.color;
			GUI.color = Color.white;
			Texture2D image;
			if (chosen)
			{
				image = Widgets.RadioButOnTex;
			}
			else
			{
				image = Widgets.RadioButOffTex;
			}
			Rect position = new Rect(x, y, 24f, 24f);
			if (disabled)
			{
				GUI.color = Color.gray;
			}
			GUI.DrawTexture(position, image);
			GUI.color = color;
		}

		// Token: 0x06003337 RID: 13111 RVA: 0x001483AA File Offset: 0x001465AA
		public static bool ButtonText(Rect rect, string label, bool drawBackground = true, bool doMouseoverSound = true, bool active = true, TextAnchor? overrideTextAnchor = null)
		{
			return Widgets.ButtonText(rect, label, drawBackground, doMouseoverSound, Widgets.NormalOptionColor, active, overrideTextAnchor);
		}

		// Token: 0x06003338 RID: 13112 RVA: 0x001483BE File Offset: 0x001465BE
		public static bool ButtonText(Rect rect, string label, bool drawBackground, bool doMouseoverSound, Color textColor, bool active = true, TextAnchor? overrideTextAnchor = null)
		{
			return Widgets.ButtonTextWorker(rect, label, drawBackground, doMouseoverSound, textColor, active, false, overrideTextAnchor).AnyPressed();
		}

		// Token: 0x06003339 RID: 13113 RVA: 0x001483D5 File Offset: 0x001465D5
		public static Widgets.DraggableResult ButtonTextDraggable(Rect rect, string label, bool drawBackground = true, bool doMouseoverSound = false, bool active = true, TextAnchor? overrideTextAnchor = null)
		{
			return Widgets.ButtonTextDraggable(rect, label, drawBackground, doMouseoverSound, Widgets.NormalOptionColor, active, overrideTextAnchor);
		}

		// Token: 0x0600333A RID: 13114 RVA: 0x001483E9 File Offset: 0x001465E9
		public static Widgets.DraggableResult ButtonTextDraggable(Rect rect, string label, bool drawBackground, bool doMouseoverSound, Color textColor, bool active = true, TextAnchor? overrideTextAnchor = null)
		{
			return Widgets.ButtonTextWorker(rect, label, drawBackground, doMouseoverSound, Widgets.NormalOptionColor, active, true, overrideTextAnchor);
		}

		// Token: 0x0600333B RID: 13115 RVA: 0x00148400 File Offset: 0x00146600
		public static void DrawButtonGraphic(Rect rect)
		{
			Texture2D atlas = Widgets.ButtonBGAtlas;
			if (Mouse.IsOver(rect))
			{
				atlas = Widgets.ButtonBGAtlasMouseover;
				if (Input.GetMouseButton(0))
				{
					atlas = Widgets.ButtonBGAtlasClick;
				}
			}
			Widgets.DrawAtlas(rect, atlas);
		}

		// Token: 0x0600333C RID: 13116 RVA: 0x00148438 File Offset: 0x00146638
		private static Widgets.DraggableResult ButtonTextWorker(Rect rect, string label, bool drawBackground, bool doMouseoverSound, Color textColor, bool active, bool draggable, TextAnchor? overrideTextAnchor = null)
		{
			TextAnchor anchor = Text.Anchor;
			Color color = GUI.color;
			if (drawBackground)
			{
				Widgets.DrawButtonGraphic(rect);
			}
			if (doMouseoverSound)
			{
				MouseoverSounds.DoRegion(rect);
			}
			if (!drawBackground)
			{
				GUI.color = textColor;
				if (Mouse.IsOver(rect))
				{
					GUI.color = Widgets.MouseoverOptionColor;
				}
			}
			if (overrideTextAnchor != null)
			{
				Text.Anchor = overrideTextAnchor.Value;
			}
			else if (drawBackground)
			{
				Text.Anchor = TextAnchor.MiddleCenter;
			}
			else
			{
				Text.Anchor = TextAnchor.MiddleLeft;
			}
			bool wordWrap = Text.WordWrap;
			if (rect.height < Text.LineHeight * 2f)
			{
				Text.WordWrap = false;
			}
			Widgets.Label(rect, label);
			Text.Anchor = anchor;
			GUI.color = color;
			Text.WordWrap = wordWrap;
			if (active && draggable)
			{
				return Widgets.ButtonInvisibleDraggable(rect, false);
			}
			if (!active)
			{
				return Widgets.DraggableResult.Idle;
			}
			if (!Widgets.ButtonInvisible(rect, false))
			{
				return Widgets.DraggableResult.Idle;
			}
			return Widgets.DraggableResult.Pressed;
		}

		// Token: 0x0600333D RID: 13117 RVA: 0x001484FF File Offset: 0x001466FF
		public static void DrawRectFast(Rect position, Color color, GUIContent content = null)
		{
			Color backgroundColor = GUI.backgroundColor;
			GUI.backgroundColor = color;
			GUI.Box(position, content ?? GUIContent.none, TexUI.FastFillStyle);
			GUI.backgroundColor = backgroundColor;
		}

		// Token: 0x0600333E RID: 13118 RVA: 0x00148528 File Offset: 0x00146728
		public static bool CustomButtonText(ref Rect rect, string label, Color bgColor, Color textColor, Color borderColor, Color unfilledBgColor = default(Color), bool cacheHeight = false, float borderSize = 1f, bool doMouseoverSound = true, bool active = true, float fillPercent = 1f)
		{
			if (cacheHeight)
			{
				Widgets.LabelCacheHeight(ref rect, label, false, false);
			}
			Rect position = new Rect(rect);
			position.x += borderSize;
			position.y += borderSize;
			position.width -= borderSize * 2f;
			position.height -= borderSize * 2f;
			Widgets.DrawRectFast(rect, borderColor, null);
			if (unfilledBgColor != default(Color))
			{
				Widgets.DrawRectFast(position, unfilledBgColor, null);
			}
			position.width *= fillPercent;
			Widgets.DrawRectFast(position, bgColor, null);
			TextAnchor anchor = Text.Anchor;
			Color color = GUI.color;
			if (doMouseoverSound)
			{
				MouseoverSounds.DoRegion(rect);
			}
			GUI.color = textColor;
			if (Mouse.IsOver(rect))
			{
				GUI.color = Widgets.MouseoverOptionColor;
			}
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, label);
			Text.Anchor = anchor;
			GUI.color = color;
			return active && Widgets.ButtonInvisible(rect, false);
		}

		// Token: 0x0600333F RID: 13119 RVA: 0x00148644 File Offset: 0x00146844
		public static bool ButtonTextSubtle(Rect rect, string label, float barPercent = 0f, float textLeftMargin = -1f, SoundDef mouseoverSound = null, Vector2 functionalSizeOffset = default(Vector2), Color? labelColor = null, bool highlight = false)
		{
			Rect rect2 = rect;
			rect2.width += functionalSizeOffset.x;
			rect2.height += functionalSizeOffset.y;
			bool flag = false;
			if (Mouse.IsOver(rect2))
			{
				flag = true;
				GUI.color = GenUI.MouseoverColor;
			}
			if (mouseoverSound != null)
			{
				MouseoverSounds.DoRegion(rect2, mouseoverSound);
			}
			Widgets.DrawAtlas(rect, Widgets.ButtonSubtleAtlas);
			if (highlight)
			{
				GUI.color = Color.grey;
				Widgets.DrawBox(rect, 2, null);
			}
			GUI.color = Color.white;
			if (barPercent > 0.001f)
			{
				Widgets.FillableBar(rect.ContractedBy(1f), barPercent, Widgets.ButtonBarTex, null, false);
			}
			Rect rect3 = new Rect(rect);
			if (textLeftMargin < 0f)
			{
				textLeftMargin = rect.width * 0.15f;
			}
			rect3.x += textLeftMargin;
			if (flag)
			{
				rect3.x += 2f;
				rect3.y -= 2f;
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Text.Font = GameFont.Small;
			GUI.color = (labelColor ?? Color.white);
			Widgets.Label(rect3, label);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.WordWrap = true;
			GUI.color = Color.white;
			return Widgets.ButtonInvisible(rect2, false);
		}

		// Token: 0x06003340 RID: 13120 RVA: 0x00148797 File Offset: 0x00146997
		public static bool ButtonImage(Rect butRect, Texture2D tex, bool doMouseoverSound = true, string tooltip = null)
		{
			return Widgets.ButtonImage(butRect, tex, Color.white, doMouseoverSound, tooltip);
		}

		// Token: 0x06003341 RID: 13121 RVA: 0x001487A7 File Offset: 0x001469A7
		public static bool ButtonImage(Rect butRect, Texture2D tex, Color baseColor, bool doMouseoverSound = true, string tooltip = null)
		{
			return Widgets.ButtonImage(butRect, tex, baseColor, GenUI.MouseoverColor, doMouseoverSound, tooltip);
		}

		// Token: 0x06003342 RID: 13122 RVA: 0x001487BC File Offset: 0x001469BC
		public static bool ButtonImage(Rect butRect, Texture2D tex, Color baseColor, Color mouseoverColor, bool doMouseoverSound = true, string tooltip = null)
		{
			GUI.color = (Mouse.IsOver(butRect) ? mouseoverColor : baseColor);
			GUI.DrawTexture(butRect, tex);
			GUI.color = baseColor;
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(butRect, tooltip);
			}
			bool result = Widgets.ButtonInvisible(butRect, doMouseoverSound);
			GUI.color = Color.white;
			return result;
		}

		// Token: 0x06003343 RID: 13123 RVA: 0x0014880F File Offset: 0x00146A0F
		public static Widgets.DraggableResult ButtonImageDraggable(Rect butRect, Texture2D tex)
		{
			return Widgets.ButtonImageDraggable(butRect, tex, Color.white);
		}

		// Token: 0x06003344 RID: 13124 RVA: 0x0014881D File Offset: 0x00146A1D
		public static Widgets.DraggableResult ButtonImageDraggable(Rect butRect, Texture2D tex, Color baseColor)
		{
			return Widgets.ButtonImageDraggable(butRect, tex, baseColor, GenUI.MouseoverColor);
		}

		// Token: 0x06003345 RID: 13125 RVA: 0x0014882C File Offset: 0x00146A2C
		public static Widgets.DraggableResult ButtonImageDraggable(Rect butRect, Texture2D tex, Color baseColor, Color mouseoverColor)
		{
			if (Mouse.IsOver(butRect))
			{
				GUI.color = mouseoverColor;
			}
			else
			{
				GUI.color = baseColor;
			}
			GUI.DrawTexture(butRect, tex);
			GUI.color = baseColor;
			return Widgets.ButtonInvisibleDraggable(butRect, false);
		}

		// Token: 0x06003346 RID: 13126 RVA: 0x00148858 File Offset: 0x00146A58
		public static bool ButtonImageFitted(Rect butRect, Texture2D tex)
		{
			return Widgets.ButtonImageFitted(butRect, tex, Color.white);
		}

		// Token: 0x06003347 RID: 13127 RVA: 0x00148866 File Offset: 0x00146A66
		public static bool ButtonImageFitted(Rect butRect, Texture2D tex, Color baseColor)
		{
			return Widgets.ButtonImageFitted(butRect, tex, baseColor, GenUI.MouseoverColor);
		}

		// Token: 0x06003348 RID: 13128 RVA: 0x00148875 File Offset: 0x00146A75
		public static bool ButtonImageFitted(Rect butRect, Texture2D tex, Color baseColor, Color mouseoverColor)
		{
			if (Mouse.IsOver(butRect))
			{
				GUI.color = mouseoverColor;
			}
			else
			{
				GUI.color = baseColor;
			}
			Widgets.DrawTextureFitted(butRect, tex, 1f, 1f);
			GUI.color = baseColor;
			return Widgets.ButtonInvisible(butRect, true);
		}

		// Token: 0x06003349 RID: 13129 RVA: 0x001488AC File Offset: 0x00146AAC
		public static bool ButtonImageWithBG(Rect butRect, Texture2D image, Vector2? imageSize = null)
		{
			bool result = Widgets.ButtonText(butRect, "", true, true, true, null);
			Rect position;
			if (imageSize != null)
			{
				position = new Rect(Mathf.Floor(butRect.x + butRect.width / 2f - imageSize.Value.x / 2f), Mathf.Floor(butRect.y + butRect.height / 2f - imageSize.Value.y / 2f), imageSize.Value.x, imageSize.Value.y);
			}
			else
			{
				position = butRect;
			}
			GUI.DrawTexture(position, image);
			return result;
		}

		// Token: 0x0600334A RID: 13130 RVA: 0x00148960 File Offset: 0x00146B60
		public static bool CloseButtonFor(Rect rectToClose)
		{
			return Widgets.ButtonImage(new Rect(rectToClose.x + rectToClose.width - 18f - 4f, rectToClose.y + 4f, 18f, 18f), TexButton.CloseXSmall, true, null);
		}

		// Token: 0x0600334B RID: 13131 RVA: 0x001489B0 File Offset: 0x00146BB0
		public static bool BackButtonFor(Rect rectToBack)
		{
			return Widgets.ButtonText(new Rect(rectToBack.x + rectToBack.width - 18f - 4f - 120f - 16f, rectToBack.y + 18f, 120f, 40f), "Back".Translate(), true, true, true, null);
		}

		// Token: 0x0600334C RID: 13132 RVA: 0x00148A20 File Offset: 0x00146C20
		public static bool ButtonInvisible(Rect butRect, bool doMouseoverSound = true)
		{
			if (doMouseoverSound)
			{
				MouseoverSounds.DoRegion(butRect);
			}
			return GUI.Button(butRect, "", Widgets.EmptyStyle);
		}

		// Token: 0x0600334D RID: 13133 RVA: 0x00148A3C File Offset: 0x00146C3C
		public static Widgets.DraggableResult ButtonInvisibleDraggable(Rect butRect, bool doMouseoverSound = false)
		{
			if (doMouseoverSound)
			{
				MouseoverSounds.DoRegion(butRect);
			}
			int controlID = GUIUtility.GetControlID(FocusType.Passive, butRect);
			if (Input.GetMouseButtonDown(0) && Mouse.IsOver(butRect))
			{
				GUIUtility.keyboardControl = 0;
				Widgets.buttonInvisibleDraggable_activeControl = controlID;
				Widgets.buttonInvisibleDraggable_mouseStart = Input.mousePosition;
				Widgets.buttonInvisibleDraggable_dragged = false;
			}
			if (Widgets.buttonInvisibleDraggable_activeControl == controlID)
			{
				if (Input.GetMouseButtonUp(0))
				{
					Widgets.buttonInvisibleDraggable_activeControl = 0;
					if (!Mouse.IsOver(butRect))
					{
						return Widgets.DraggableResult.Idle;
					}
					if (!Widgets.buttonInvisibleDraggable_dragged)
					{
						return Widgets.DraggableResult.Pressed;
					}
					return Widgets.DraggableResult.DraggedThenPressed;
				}
				else
				{
					if (!Input.GetMouseButton(0))
					{
						Widgets.buttonInvisibleDraggable_activeControl = 0;
						return Widgets.DraggableResult.Idle;
					}
					if (!Widgets.buttonInvisibleDraggable_dragged && (Widgets.buttonInvisibleDraggable_mouseStart - Input.mousePosition).sqrMagnitude > Widgets.DragStartDistanceSquared)
					{
						Widgets.buttonInvisibleDraggable_dragged = true;
						return Widgets.DraggableResult.Dragged;
					}
				}
			}
			return Widgets.DraggableResult.Idle;
		}

		// Token: 0x0600334E RID: 13134 RVA: 0x00148AEF File Offset: 0x00146CEF
		public static string TextField(Rect rect, string text)
		{
			if (text == null)
			{
				text = "";
			}
			return GUI.TextField(rect, text, Text.CurTextFieldStyle);
		}

		// Token: 0x0600334F RID: 13135 RVA: 0x00148B08 File Offset: 0x00146D08
		public static string TextField(Rect rect, string text, int maxLength, Regex inputValidator = null)
		{
			string text2 = Widgets.TextField(rect, text);
			if (text2.Length <= maxLength && (inputValidator == null || inputValidator.IsMatch(text2)))
			{
				return text2;
			}
			return text;
		}

		// Token: 0x06003350 RID: 13136 RVA: 0x00148B35 File Offset: 0x00146D35
		public static string TextArea(Rect rect, string text, bool readOnly = false)
		{
			if (text == null)
			{
				text = "";
			}
			return GUI.TextArea(rect, text, readOnly ? Text.CurTextAreaReadOnlyStyle : Text.CurTextAreaStyle);
		}

		// Token: 0x06003351 RID: 13137 RVA: 0x00148B58 File Offset: 0x00146D58
		public static string TextEntryLabeled(Rect rect, string label, string text)
		{
			Rect rect2 = rect.LeftHalf().Rounded();
			Rect rect3 = rect.RightHalf().Rounded();
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect2, label);
			Text.Anchor = anchor;
			if (rect.height <= 30f)
			{
				return Widgets.TextField(rect3, text);
			}
			return Widgets.TextArea(rect3, text, false);
		}

		// Token: 0x06003352 RID: 13138 RVA: 0x00148BB4 File Offset: 0x00146DB4
		public static string DelayedTextField(Rect rect, string text, ref string buffer, string previousFocusedControlName, string controlName = null)
		{
			string text2;
			if ((text2 = controlName) == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(10, 2);
				defaultInterpolatedStringHandler.AppendLiteral("TextField");
				defaultInterpolatedStringHandler.AppendFormatted<float>(rect.x);
				defaultInterpolatedStringHandler.AppendLiteral(",");
				defaultInterpolatedStringHandler.AppendFormatted<float>(rect.y);
				text2 = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			controlName = text2;
			bool flag = previousFocusedControlName == controlName;
			bool flag2 = GUI.GetNameOfFocusedControl() == controlName;
			string text3 = controlName + "_unfocused";
			GUI.SetNextControlName(text3);
			GUI.Label(rect, "");
			GUI.SetNextControlName(controlName);
			bool flag3 = false;
			if (flag2 && Event.current.type == EventType.keyDown && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter))
			{
				Event.current.Use();
				flag3 = true;
			}
			bool flag4 = false;
			if (Event.current.type == EventType.mouseDown && !rect.Contains(Event.current.mousePosition))
			{
				flag4 = true;
			}
			if (!flag)
			{
				buffer = Widgets.TextField(rect, text);
				return buffer;
			}
			buffer = Widgets.TextField(rect, buffer);
			if (!flag2)
			{
				return buffer;
			}
			if (flag4 || flag3)
			{
				GUI.FocusControl(text3);
				return buffer;
			}
			return text;
		}

		// Token: 0x06003353 RID: 13139 RVA: 0x00148CD8 File Offset: 0x00146ED8
		public static void TextFieldVector(Rect rect, ref Vector3 vector, ref string[] buffer, float min = 0f, float max = 1E+09f)
		{
			if (buffer == null)
			{
				buffer = new string[3];
			}
			float width = rect.width / 3f - 4f;
			Rect rect2 = rect.LeftPartPixels(width);
			Rect rect3 = rect2;
			Rect rect4 = rect3;
			rect3.x = rect2.xMax + 4f;
			rect4.x = rect3.xMax + 4f;
			Widgets.TextFieldNumeric<float>(rect2, ref vector.x, ref buffer[0], min, max);
			Widgets.TextFieldNumeric<float>(rect3, ref vector.y, ref buffer[1], min, max);
			Widgets.TextFieldNumeric<float>(rect4, ref vector.z, ref buffer[2], min, max);
		}

		// Token: 0x06003354 RID: 13140 RVA: 0x00148D80 File Offset: 0x00146F80
		public static void TextFieldNumeric<T>(Rect rect, ref T val, ref string buffer, float min = 0f, float max = 1E+09f) where T : struct
		{
			if (buffer == null)
			{
				buffer = val.ToString();
			}
			GUI.SetNextControlName("TextField" + rect.y.ToString("F0") + rect.x.ToString("F0"));
			string text = Widgets.TextField(rect, buffer);
			if (text != buffer && Widgets.IsPartiallyOrFullyTypedNumber<T>(ref val, text, min, max))
			{
				buffer = text;
				if (text.IsFullyTypedNumber<T>())
				{
					Widgets.ResolveParseNow<T>(text, ref val, ref buffer, min, max, false);
				}
			}
		}

		// Token: 0x06003355 RID: 13141 RVA: 0x00148E0C File Offset: 0x0014700C
		private static void ResolveParseNow<T>(string edited, ref T val, ref string buffer, float min, float max, bool force)
		{
			if (typeof(T) == typeof(int))
			{
				if (edited.NullOrEmpty())
				{
					Widgets.ResetValue<T>(edited, ref val, ref buffer, min, max);
					return;
				}
				int num;
				if (int.TryParse(edited, out num))
				{
					val = (T)((object)Mathf.RoundToInt(Mathf.Clamp((float)num, min, max)));
					buffer = Widgets.ToStringTypedIn<T>(val);
					return;
				}
				if (force)
				{
					Widgets.ResetValue<T>(edited, ref val, ref buffer, min, max);
					return;
				}
			}
			else if (typeof(T) == typeof(float))
			{
				float value;
				if (float.TryParse(edited, out value))
				{
					val = (T)((object)Mathf.Clamp(value, min, max));
					buffer = Widgets.ToStringTypedIn<T>(val);
					return;
				}
				if (force)
				{
					Widgets.ResetValue<T>(edited, ref val, ref buffer, min, max);
					return;
				}
			}
			else
			{
				string str = "TextField<T> does not support ";
				Type typeFromHandle = typeof(T);
				Log.Error(str + ((typeFromHandle != null) ? typeFromHandle.ToString() : null));
			}
		}

		// Token: 0x06003356 RID: 13142 RVA: 0x00148F14 File Offset: 0x00147114
		private static void ResetValue<T>(string edited, ref T val, ref string buffer, float min, float max)
		{
			val = default(T);
			if (min > 0f)
			{
				val = (T)((object)Mathf.RoundToInt(min));
			}
			if (max < 0f)
			{
				val = (T)((object)Mathf.RoundToInt(max));
			}
			buffer = Widgets.ToStringTypedIn<T>(val);
		}

		// Token: 0x06003357 RID: 13143 RVA: 0x00148F74 File Offset: 0x00147174
		private static string ToStringTypedIn<T>(T val)
		{
			if (typeof(T) == typeof(float))
			{
				return ((float)((object)val)).ToString("0.##########");
			}
			return val.ToString();
		}

		// Token: 0x06003358 RID: 13144 RVA: 0x00148FC4 File Offset: 0x001471C4
		private static bool IsPartiallyOrFullyTypedNumber<T>(ref T val, string s, float min, float max)
		{
			return s == "" || ((s[0] != '-' || min < 0f) && (s.Length <= 1 || s[s.Length - 1] != '-') && !(s == "00") && s.Length <= 12 && ((typeof(T) == typeof(float) && s.CharacterCount('.') <= 1 && s.ContainsOnlyCharacters("-.0123456789")) || s.IsFullyTypedNumber<T>()));
		}

		// Token: 0x06003359 RID: 13145 RVA: 0x00149070 File Offset: 0x00147270
		private static bool IsFullyTypedNumber<T>(this string s)
		{
			if (s == "")
			{
				return false;
			}
			if (typeof(T) == typeof(float))
			{
				string[] array = s.Split('.', StringSplitOptions.None);
				if (array.Length > 2 || array.Length < 1)
				{
					return false;
				}
				if (!array[0].ContainsOnlyCharacters("-0123456789"))
				{
					return false;
				}
				if (array.Length == 2 && (array[1].Length == 0 || !array[1].ContainsOnlyCharacters("0123456789")))
				{
					return false;
				}
			}
			return !(typeof(T) == typeof(int)) || s.ContainsOnlyCharacters("-0123456789");
		}

		// Token: 0x0600335A RID: 13146 RVA: 0x0014911C File Offset: 0x0014731C
		private static bool ContainsOnlyCharacters(this string s, string allowedChars)
		{
			for (int i = 0; i < s.Length; i++)
			{
				if (!allowedChars.Contains(s[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600335B RID: 13147 RVA: 0x0014914C File Offset: 0x0014734C
		private static int CharacterCount(this string s, char c)
		{
			int num = 0;
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] == c)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600335C RID: 13148 RVA: 0x0014917C File Offset: 0x0014737C
		public static void TextFieldNumericLabeled<T>(Rect rect, string label, ref T val, ref string buffer, float min = 0f, float max = 1E+09f) where T : struct
		{
			Rect rect2 = rect.LeftHalf().Rounded();
			Rect rect3 = rect.RightHalf().Rounded();
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect2, label);
			Text.Anchor = anchor;
			Widgets.TextFieldNumeric<T>(rect3, ref val, ref buffer, min, max);
		}

		// Token: 0x0600335D RID: 13149 RVA: 0x001491C4 File Offset: 0x001473C4
		public static void TextFieldPercent(Rect rect, ref float val, ref string buffer, float min = 0f, float max = 1f)
		{
			Rect rect2 = new Rect(rect.x, rect.y, rect.width - 25f, rect.height);
			Widgets.Label(new Rect(rect2.xMax, rect.y, 25f, rect2.height), "%");
			float num = val * 100f;
			Widgets.TextFieldNumeric<float>(rect2, ref num, ref buffer, min * 100f, max * 100f);
			val = num / 100f;
			if (val > max)
			{
				val = max;
				buffer = val.ToString();
			}
		}

		// Token: 0x0600335E RID: 13150 RVA: 0x00149260 File Offset: 0x00147460
		public static T ChangeType<T>(this object obj)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return (T)((object)Convert.ChangeType(obj, typeof(T), invariantCulture));
		}

		// Token: 0x0600335F RID: 13151 RVA: 0x00149289 File Offset: 0x00147489
		public static void ResetSliderDraggingIDs()
		{
			Widgets.sliderDraggingID = 0;
			Widgets.draggingId = 0;
			Widgets.curDragEnd = Widgets.RangeEnd.None;
		}

		// Token: 0x06003360 RID: 13152 RVA: 0x001492A0 File Offset: 0x001474A0
		public static void HorizontalSlider(Rect rect, ref float value, FloatRange range, string label = null, float roundTo = -1f)
		{
			float trueMin = range.TrueMin;
			float trueMax = range.TrueMax;
			value = Widgets.HorizontalSlider(rect, value, trueMin, trueMax, false, label, trueMin.ToString(), trueMax.ToString(), roundTo);
		}

		// Token: 0x06003361 RID: 13153 RVA: 0x001492DC File Offset: 0x001474DC
		public static float HorizontalSlider(Rect rect, float value, float min, float max, bool middleAlignment = false, string label = null, string leftAlignedLabel = null, string rightAlignedLabel = null, float roundTo = -1f)
		{
			float num = value;
			if (middleAlignment || !label.NullOrEmpty())
			{
				rect.y += Mathf.Round((rect.height - 10f) / 2f);
			}
			if (!label.NullOrEmpty())
			{
				rect.y += 5f;
			}
			int num2 = UI.GUIToScreenPoint(new Vector2(rect.x, rect.y)).GetHashCode();
			num2 = Gen.HashCombine<float>(num2, rect.width);
			num2 = Gen.HashCombine<float>(num2, rect.height);
			num2 = Gen.HashCombine<float>(num2, min);
			num2 = Gen.HashCombine<float>(num2, max);
			Rect rect2 = rect;
			rect2.xMin += 6f;
			rect2.xMax -= 6f;
			GUI.color = Widgets.RangeControlTextColor;
			Rect rect3 = new Rect(rect2.x, rect2.y + 2f, rect2.width, 8f);
			Widgets.DrawAtlas(rect3, Widgets.SliderRailAtlas);
			GUI.color = Color.white;
			float x = Mathf.Clamp(rect2.x - 6f + rect2.width * Mathf.InverseLerp(min, max, num), rect2.xMin - 6f, rect2.xMax - 6f);
			GUI.DrawTexture(new Rect(x, rect3.center.y - 6f, 12f, 12f), Widgets.SliderHandle);
			if (Event.current.type == EventType.mouseDown && Mouse.IsOver(rect) && Widgets.sliderDraggingID != num2)
			{
				Widgets.sliderDraggingID = num2;
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				Event.current.Use();
			}
			if (Widgets.sliderDraggingID == num2 && UnityGUIBugsFixer.MouseDrag(0))
			{
				num = Mathf.Clamp((Event.current.mousePosition.x - rect2.x) / rect2.width * (max - min) + min, min, max);
				if (Event.current.type == EventType.mouseDrag)
				{
					Event.current.Use();
				}
			}
			if (!label.NullOrEmpty() || !leftAlignedLabel.NullOrEmpty() || !rightAlignedLabel.NullOrEmpty())
			{
				TextAnchor anchor = Text.Anchor;
				GameFont font = Text.Font;
				Text.Font = GameFont.Small;
				float num3 = label.NullOrEmpty() ? 18f : Text.CalcSize(label).y;
				rect.y = rect.y - num3 + 3f;
				if (!leftAlignedLabel.NullOrEmpty())
				{
					Text.Anchor = TextAnchor.UpperLeft;
					Widgets.Label(rect, leftAlignedLabel);
				}
				if (!rightAlignedLabel.NullOrEmpty())
				{
					Text.Anchor = TextAnchor.UpperRight;
					Widgets.Label(rect, rightAlignedLabel);
				}
				if (!label.NullOrEmpty())
				{
					Text.Anchor = TextAnchor.UpperCenter;
					Widgets.Label(rect, label);
				}
				Text.Anchor = anchor;
				Text.Font = font;
			}
			if (roundTo > 0f)
			{
				num = (float)Mathf.RoundToInt(num / roundTo) * roundTo;
			}
			if (value != num)
			{
				Widgets.CheckPlayDragSliderSound();
			}
			return num;
		}

		// Token: 0x06003362 RID: 13154 RVA: 0x001495CC File Offset: 0x001477CC
		public static float FrequencyHorizontalSlider(Rect rect, float freq, float minFreq, float maxFreq, bool roundToInt = false)
		{
			float num;
			if (freq < 1f)
			{
				float x = 1f / freq;
				num = GenMath.LerpDouble(1f, 1f / minFreq, 0.5f, 1f, x);
			}
			else
			{
				num = GenMath.LerpDouble(maxFreq, 1f, 0f, 0.5f, freq);
			}
			string label;
			if (freq == 1f)
			{
				label = "EveryDay".Translate();
			}
			else if (freq < 1f)
			{
				label = "TimesPerDay".Translate((1f / freq).ToString("0.##"));
			}
			else
			{
				label = "EveryDays".Translate(freq.ToString("0.##"));
			}
			float num2 = Widgets.HorizontalSlider(rect, num, 0f, 1f, true, label, null, null, -1f);
			if (num != num2)
			{
				float num3;
				if (num2 < 0.5f)
				{
					num3 = GenMath.LerpDouble(0.5f, 0f, 1f, maxFreq, num2);
					if (roundToInt)
					{
						num3 = Mathf.Round(num3);
					}
				}
				else
				{
					float num4 = GenMath.LerpDouble(1f, 0.5f, 1f / minFreq, 1f, num2);
					if (roundToInt)
					{
						num4 = Mathf.Round(num4);
					}
					num3 = 1f / num4;
				}
				freq = num3;
			}
			return freq;
		}

		// Token: 0x06003363 RID: 13155 RVA: 0x00149718 File Offset: 0x00147918
		public static void IntEntry(Rect rect, ref int value, ref string editBuffer, int multiplier = 1)
		{
			int num = Mathf.Min(Widgets.IntEntryButtonWidth, (int)rect.width / 5);
			if (Widgets.ButtonText(new Rect(rect.xMin, rect.yMin, (float)num, rect.height), (-10 * multiplier).ToStringCached(), true, true, true, null))
			{
				value -= 10 * multiplier * GenUI.CurrentAdjustmentMultiplier();
				editBuffer = value.ToStringCached();
				SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
			}
			if (Widgets.ButtonText(new Rect(rect.xMin + (float)num, rect.yMin, (float)num, rect.height), (-1 * multiplier).ToStringCached(), true, true, true, null))
			{
				value -= multiplier * GenUI.CurrentAdjustmentMultiplier();
				editBuffer = value.ToStringCached();
				SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
			}
			if (Widgets.ButtonText(new Rect(rect.xMax - (float)num, rect.yMin, (float)num, rect.height), "+" + (10 * multiplier).ToStringCached(), true, true, true, null))
			{
				value += 10 * multiplier * GenUI.CurrentAdjustmentMultiplier();
				editBuffer = value.ToStringCached();
				SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
			}
			if (Widgets.ButtonText(new Rect(rect.xMax - (float)(num * 2), rect.yMin, (float)num, rect.height), "+" + multiplier.ToStringCached(), true, true, true, null))
			{
				value += multiplier * GenUI.CurrentAdjustmentMultiplier();
				editBuffer = value.ToStringCached();
				SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
			}
			Widgets.TextFieldNumeric<int>(new Rect(rect.xMin + (float)(num * 2), rect.yMin, rect.width - (float)(num * 4), rect.height), ref value, ref editBuffer, 0f, 1E+09f);
		}

		// Token: 0x06003364 RID: 13156 RVA: 0x001498F8 File Offset: 0x00147AF8
		public static void FloatRange(Rect rect, int id, ref FloatRange range, float min = 0f, float max = 1f, string labelKey = null, ToStringStyle valueStyle = ToStringStyle.FloatTwo, float gap = 0f, GameFont sliderLabelFont = GameFont.Small, Color? sliderLabelColor = null, float roundTo = 0f)
		{
			Rect rect2 = rect;
			rect2.xMin += 8f;
			rect2.xMax -= 8f;
			GUI.color = (sliderLabelColor ?? Widgets.RangeControlTextColor);
			string text = range.min.ToStringByStyle(valueStyle, ToStringNumberSense.Absolute) + " - " + range.max.ToStringByStyle(valueStyle, ToStringNumberSense.Absolute);
			if (labelKey != null)
			{
				text = labelKey.Translate(text);
			}
			GameFont font = Text.Font;
			Text.Font = sliderLabelFont;
			Text.Anchor = TextAnchor.UpperCenter;
			Rect rect3 = rect2;
			rect3.yMin -= 2f;
			rect3.height = Mathf.Max(rect3.height, Text.CalcHeight(text, rect3.width));
			Widgets.LabelFit(rect3, text);
			Text.Anchor = TextAnchor.UpperLeft;
			Rect position = new Rect(rect2.x, rect2.yMax - 8f - 1f, rect2.width, 2f);
			GUI.DrawTexture(position, BaseContent.WhiteTex);
			float num = rect2.x + rect2.width * Mathf.InverseLerp(min, max, range.min);
			float num2 = rect2.x + rect2.width * Mathf.InverseLerp(min, max, range.max);
			GUI.color = Color.white;
			GUI.DrawTexture(new Rect(num, rect2.yMax - 8f - 2f, num2 - num, 4f), BaseContent.WhiteTex);
			float num3 = num;
			float num4 = num2;
			Rect position2 = new Rect(num3 - 16f, position.center.y - 8f, 16f, 16f);
			GUI.DrawTexture(position2, Widgets.FloatRangeSliderTex);
			Rect position3 = new Rect(num4 + 16f, position.center.y - 8f, -16f, 16f);
			GUI.DrawTexture(position3, Widgets.FloatRangeSliderTex);
			if (Widgets.curDragEnd != Widgets.RangeEnd.None && (Event.current.type == EventType.mouseUp || Event.current.rawType == EventType.mouseDown))
			{
				Widgets.draggingId = 0;
				Widgets.curDragEnd = Widgets.RangeEnd.None;
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				Event.current.Use();
			}
			bool flag = false;
			if (Mouse.IsOver(rect) || Widgets.draggingId == id)
			{
				if (Event.current.type == EventType.mouseDown && Event.current.button == 0 && id != Widgets.draggingId)
				{
					Widgets.draggingId = id;
					float x = Event.current.mousePosition.x;
					if (x < position2.xMax)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Min;
					}
					else if (x > position3.xMin)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Max;
					}
					else
					{
						float num5 = Mathf.Abs(x - position2.xMax);
						float num6 = Mathf.Abs(x - (position3.x - 16f));
						Widgets.curDragEnd = ((num5 < num6) ? Widgets.RangeEnd.Min : Widgets.RangeEnd.Max);
					}
					flag = true;
					Event.current.Use();
					SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				}
				if (flag || (Widgets.curDragEnd != Widgets.RangeEnd.None && UnityGUIBugsFixer.MouseDrag(0)))
				{
					float num7 = (Event.current.mousePosition.x - rect2.x) / rect2.width * (max - min) + min;
					num7 = Mathf.Clamp(num7, min, max);
					if (Widgets.curDragEnd == Widgets.RangeEnd.Min)
					{
						if (num7 != range.min)
						{
							range.min = Mathf.Min(num7, max - gap);
							if (range.max < range.min + gap)
							{
								range.max = range.min + gap;
							}
							Widgets.CheckPlayDragSliderSound();
						}
					}
					else if (Widgets.curDragEnd == Widgets.RangeEnd.Max && num7 != range.max)
					{
						range.max = Mathf.Max(num7, min + gap);
						if (range.min > range.max - gap)
						{
							range.min = range.max - gap;
						}
						Widgets.CheckPlayDragSliderSound();
					}
					if (roundTo != 0f)
					{
						range.min = Mathf.Round(range.min / roundTo) * roundTo;
						range.max = Mathf.Round(range.max / roundTo) * roundTo;
					}
					if (Event.current.type == EventType.mouseDrag)
					{
						Event.current.Use();
					}
				}
			}
			Text.Font = font;
		}

		// Token: 0x06003365 RID: 13157 RVA: 0x00149D48 File Offset: 0x00147F48
		public static void IntRange(Rect rect, int id, ref IntRange range, int min = 0, int max = 100, string labelKey = null, int minWidth = 0)
		{
			Rect rect2 = rect;
			rect2.xMin += 8f;
			rect2.xMax -= 8f;
			GUI.color = Widgets.RangeControlTextColor;
			string text = range.min.ToStringCached() + " - " + range.max.ToStringCached();
			if (labelKey != null)
			{
				text = labelKey.Translate(text);
			}
			GameFont font = Text.Font;
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperCenter;
			Rect rect3 = rect2;
			rect3.yMin -= 2f;
			Widgets.Label(rect3, text);
			Text.Anchor = TextAnchor.UpperLeft;
			Rect position = new Rect(rect2.x, rect2.yMax - 8f - 1f, rect2.width, 2f);
			GUI.DrawTexture(position, BaseContent.WhiteTex);
			float num = rect2.x + rect2.width * (float)(range.min - min) / (float)(max - min);
			float num2 = rect2.x + rect2.width * (float)(range.max - min) / (float)(max - min);
			GUI.color = Color.white;
			GUI.DrawTexture(new Rect(num, rect2.yMax - 8f - 2f, num2 - num, 4f), BaseContent.WhiteTex);
			float num3 = num;
			float num4 = num2;
			Rect position2 = new Rect(num3 - 16f, position.center.y - 8f, 16f, 16f);
			GUI.DrawTexture(position2, Widgets.FloatRangeSliderTex);
			Rect position3 = new Rect(num4 + 16f, position.center.y - 8f, -16f, 16f);
			GUI.DrawTexture(position3, Widgets.FloatRangeSliderTex);
			if (Widgets.curDragEnd != Widgets.RangeEnd.None && (Event.current.type == EventType.mouseUp || Event.current.rawType == EventType.mouseDown))
			{
				Widgets.draggingId = 0;
				Widgets.curDragEnd = Widgets.RangeEnd.None;
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
			}
			bool flag = false;
			if (Mouse.IsOver(rect) || Widgets.draggingId == id)
			{
				if (Event.current.type == EventType.mouseDown && Event.current.button == 0 && id != Widgets.draggingId)
				{
					Widgets.draggingId = id;
					float x = Event.current.mousePosition.x;
					if (x < position2.xMax)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Min;
					}
					else if (x > position3.xMin)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Max;
					}
					else
					{
						float num5 = Mathf.Abs(x - position2.xMax);
						float num6 = Mathf.Abs(x - (position3.x - 16f));
						Widgets.curDragEnd = ((num5 < num6) ? Widgets.RangeEnd.Min : Widgets.RangeEnd.Max);
					}
					flag = true;
					Event.current.Use();
					SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				}
				if (flag || (Widgets.curDragEnd != Widgets.RangeEnd.None && UnityGUIBugsFixer.MouseDrag(0)))
				{
					int num7 = Mathf.RoundToInt(Mathf.Clamp((Event.current.mousePosition.x - rect2.x) / rect2.width * (float)(max - min) + (float)min, (float)min, (float)max));
					if (Widgets.curDragEnd == Widgets.RangeEnd.Min)
					{
						if (num7 != range.min)
						{
							range.min = num7;
							if (range.min > max - minWidth)
							{
								range.min = max - minWidth;
							}
							int num8 = Mathf.Max(min, range.min + minWidth);
							if (range.max < num8)
							{
								range.max = num8;
							}
							Widgets.CheckPlayDragSliderSound();
						}
					}
					else if (Widgets.curDragEnd == Widgets.RangeEnd.Max && num7 != range.max)
					{
						range.max = num7;
						if (range.max < min + minWidth)
						{
							range.max = min + minWidth;
						}
						int num9 = Mathf.Min(max, range.max - minWidth);
						if (range.min > num9)
						{
							range.min = num9;
						}
						Widgets.CheckPlayDragSliderSound();
					}
					if (Event.current.type == EventType.mouseDrag)
					{
						Event.current.Use();
					}
				}
			}
			Text.Font = font;
		}

		// Token: 0x06003366 RID: 13158 RVA: 0x0014A142 File Offset: 0x00148342
		private static void CheckPlayDragSliderSound()
		{
			if (Time.realtimeSinceStartup > Widgets.lastDragSliderSoundTime + 0.075f)
			{
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				Widgets.lastDragSliderSoundTime = Time.realtimeSinceStartup;
			}
		}

		// Token: 0x06003367 RID: 13159 RVA: 0x0014A16C File Offset: 0x0014836C
		public static void QualityRange(Rect rect, int id, ref QualityRange range)
		{
			Rect rect2 = rect;
			rect2.xMin += 8f;
			rect2.xMax -= 8f;
			GUI.color = Widgets.RangeControlTextColor;
			string label;
			if (range == RimWorld.QualityRange.All)
			{
				label = "AnyQuality".Translate();
			}
			else if (range.max == range.min)
			{
				label = "OnlyQuality".Translate(range.min.GetLabel());
			}
			else
			{
				label = range.min.GetLabel() + " - " + range.max.GetLabel();
			}
			GameFont font = Text.Font;
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperCenter;
			Rect rect3 = rect2;
			rect3.yMin -= 2f;
			Widgets.Label(rect3, label);
			Text.Anchor = TextAnchor.UpperLeft;
			Rect position = new Rect(rect2.x, rect2.yMax - 8f - 1f, rect2.width, 2f);
			GUI.DrawTexture(position, BaseContent.WhiteTex);
			int qualityCount = QualityUtility.QualityCount;
			float num = rect2.x + rect2.width / (float)(qualityCount - 1) * (float)range.min;
			float num2 = rect2.x + rect2.width / (float)(qualityCount - 1) * (float)range.max;
			GUI.color = Color.white;
			GUI.DrawTexture(new Rect(num, rect2.yMax - 8f - 2f, num2 - num, 4f), BaseContent.WhiteTex);
			float num3 = num;
			float num4 = num2;
			Rect position2 = new Rect(num3 - 16f, position.center.y - 8f, 16f, 16f);
			GUI.DrawTexture(position2, Widgets.FloatRangeSliderTex);
			Rect position3 = new Rect(num4 + 16f, position.center.y - 8f, -16f, 16f);
			GUI.DrawTexture(position3, Widgets.FloatRangeSliderTex);
			if (Widgets.curDragEnd != Widgets.RangeEnd.None && (Event.current.type == EventType.mouseUp || Event.current.type == EventType.mouseDown))
			{
				Widgets.draggingId = 0;
				Widgets.curDragEnd = Widgets.RangeEnd.None;
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
			}
			bool flag = false;
			if (Mouse.IsOver(rect) || id == Widgets.draggingId)
			{
				if (Event.current.type == EventType.mouseDown && Event.current.button == 0 && id != Widgets.draggingId)
				{
					Widgets.draggingId = id;
					float x = Event.current.mousePosition.x;
					if (x < position2.xMax)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Min;
					}
					else if (x > position3.xMin)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Max;
					}
					else
					{
						float num5 = Mathf.Abs(x - position2.xMax);
						float num6 = Mathf.Abs(x - (position3.x - 16f));
						Widgets.curDragEnd = ((num5 < num6) ? Widgets.RangeEnd.Min : Widgets.RangeEnd.Max);
					}
					flag = true;
					Event.current.Use();
					SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				}
				if (flag || (Widgets.curDragEnd != Widgets.RangeEnd.None && UnityGUIBugsFixer.MouseDrag(0)))
				{
					int num7 = Mathf.RoundToInt((Event.current.mousePosition.x - rect2.x) / rect2.width * (float)(qualityCount - 1));
					num7 = Mathf.Clamp(num7, 0, qualityCount - 1);
					if (Widgets.curDragEnd == Widgets.RangeEnd.Min)
					{
						if (range.min != (QualityCategory)num7)
						{
							range.min = (QualityCategory)num7;
							if (range.max < range.min)
							{
								range.max = range.min;
							}
							SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
						}
					}
					else if (Widgets.curDragEnd == Widgets.RangeEnd.Max && range.max != (QualityCategory)num7)
					{
						range.max = (QualityCategory)num7;
						if (range.min > range.max)
						{
							range.min = range.max;
						}
						SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
					}
					if (Event.current.type == EventType.mouseDrag)
					{
						Event.current.Use();
					}
				}
			}
			Text.Font = font;
		}

		// Token: 0x06003368 RID: 13160 RVA: 0x0014A574 File Offset: 0x00148774
		public static void FloatRangeWithTypeIn(Rect rect, int id, ref FloatRange fRange, float sliderMin = 0f, float sliderMax = 1f, ToStringStyle valueStyle = ToStringStyle.FloatTwo, string labelKey = null)
		{
			Rect rect2 = new Rect(rect);
			rect2.width = rect.width / 4f;
			Rect rect3 = new Rect(rect);
			rect3.width = rect.width / 2f;
			rect3.x = rect.x + rect.width / 4f;
			rect3.height = rect.height / 2f;
			rect3.width -= rect.height;
			Rect butRect = new Rect(rect3);
			butRect.x = rect3.xMax;
			butRect.height = rect.height;
			butRect.width = rect.height;
			Rect rect4 = new Rect(rect);
			rect4.x = rect.x + rect.width * 0.75f;
			rect4.width = rect.width / 4f;
			rect3.y += 4f;
			rect3.height += 4f;
			Widgets.FloatRange(rect3, id, ref fRange, sliderMin, sliderMax, labelKey, valueStyle, 0f, GameFont.Small, null, 0f);
			if (Widgets.ButtonImage(butRect, TexButton.RangeMatch, true, null))
			{
				fRange.max = fRange.min;
			}
			float.TryParse(Widgets.TextField(rect2, fRange.min.ToString()), out fRange.min);
			float.TryParse(Widgets.TextField(rect4, fRange.max.ToString()), out fRange.max);
		}

		// Token: 0x06003369 RID: 13161 RVA: 0x0014A707 File Offset: 0x00148907
		public static Rect FillableBar(Rect rect, float fillPercent)
		{
			return Widgets.FillableBar(rect, fillPercent, Widgets.BarFullTexHor);
		}

		// Token: 0x0600336A RID: 13162 RVA: 0x0014A718 File Offset: 0x00148918
		public static Rect FillableBar(Rect rect, float fillPercent, Texture2D fillTex)
		{
			bool doBorder = rect.height > 15f && rect.width > 20f;
			return Widgets.FillableBar(rect, fillPercent, fillTex, Widgets.DefaultBarBgTex, doBorder);
		}

		// Token: 0x0600336B RID: 13163 RVA: 0x0014A754 File Offset: 0x00148954
		public static Rect FillableBar(Rect rect, float fillPercent, Texture2D fillTex, Texture2D bgTex, bool doBorder)
		{
			if (doBorder)
			{
				GUI.DrawTexture(rect, BaseContent.BlackTex);
				rect = rect.ContractedBy(3f);
			}
			if (bgTex != null)
			{
				GUI.DrawTexture(rect, bgTex);
			}
			Rect result = rect;
			rect.width *= fillPercent;
			GUI.DrawTexture(rect, fillTex);
			return result;
		}

		// Token: 0x0600336C RID: 13164 RVA: 0x0014A7A4 File Offset: 0x001489A4
		public static void FillableBarLabeled(Rect rect, float fillPercent, int labelWidth, string label)
		{
			if (fillPercent < 0f)
			{
				fillPercent = 0f;
			}
			if (fillPercent > 1f)
			{
				fillPercent = 1f;
			}
			Rect rect2 = rect;
			rect2.width = (float)labelWidth;
			Widgets.Label(rect2, label);
			Rect rect3 = rect;
			rect3.x += (float)labelWidth;
			rect3.width -= (float)labelWidth;
			Widgets.FillableBar(rect3, fillPercent);
		}

		// Token: 0x0600336D RID: 13165 RVA: 0x0014A80C File Offset: 0x00148A0C
		public static void FillableBarChangeArrows(Rect barRect, float changeRate)
		{
			int changeRate2 = (int)(changeRate * Widgets.FillableBarChangeRateDisplayRatio);
			Widgets.FillableBarChangeArrows(barRect, changeRate2);
		}

		// Token: 0x0600336E RID: 13166 RVA: 0x0014A82C File Offset: 0x00148A2C
		public static void FillableBarChangeArrows(Rect barRect, int changeRate)
		{
			if (changeRate == 0)
			{
				return;
			}
			if (changeRate > Widgets.MaxFillableBarChangeRate)
			{
				changeRate = Widgets.MaxFillableBarChangeRate;
			}
			if (changeRate < -Widgets.MaxFillableBarChangeRate)
			{
				changeRate = -Widgets.MaxFillableBarChangeRate;
			}
			float num = barRect.height;
			if (num > 16f)
			{
				num = 16f;
			}
			int num2 = Mathf.Abs(changeRate);
			float y = barRect.y + barRect.height / 2f - num / 2f;
			float num3;
			float num4;
			Texture2D image;
			if (changeRate > 0)
			{
				num3 = barRect.x + barRect.width + 2f;
				num4 = 8f;
				image = Widgets.FillArrowTexRight;
			}
			else
			{
				num3 = barRect.x - 8f - 2f;
				num4 = -8f;
				image = Widgets.FillArrowTexLeft;
			}
			for (int i = 0; i < num2; i++)
			{
				GUI.DrawTexture(new Rect(num3, y, 8f, num), image);
				num3 += num4;
			}
		}

		// Token: 0x0600336F RID: 13167 RVA: 0x0014A90D File Offset: 0x00148B0D
		public static void DrawWindowBackground(Rect rect)
		{
			GUI.color = Widgets.WindowBGFillColor;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = Widgets.WindowBGBorderColor;
			Widgets.DrawBox(rect, 1, null);
			GUI.color = Color.white;
		}

		// Token: 0x06003370 RID: 13168 RVA: 0x0014A940 File Offset: 0x00148B40
		public static void DrawWindowBackground(Rect rect, Color colorFactor)
		{
			Color color = GUI.color;
			GUI.color = Widgets.WindowBGFillColor * colorFactor;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = Widgets.WindowBGBorderColor * colorFactor;
			Widgets.DrawBox(rect, 1, null);
			GUI.color = color;
		}

		// Token: 0x06003371 RID: 13169 RVA: 0x0014A97F File Offset: 0x00148B7F
		public static void DrawMenuSection(Rect rect)
		{
			GUI.color = Widgets.MenuSectionBGFillColor;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = Widgets.MenuSectionBGBorderColor;
			Widgets.DrawBox(rect, 1, null);
			GUI.color = Color.white;
		}

		// Token: 0x06003372 RID: 13170 RVA: 0x0014A9B2 File Offset: 0x00148BB2
		public static void DrawWindowBackgroundTutor(Rect rect)
		{
			GUI.color = Widgets.TutorWindowBGFillColor;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = Widgets.TutorWindowBGBorderColor;
			Widgets.DrawBox(rect, 1, null);
			GUI.color = Color.white;
		}

		// Token: 0x06003373 RID: 13171 RVA: 0x0014A9E5 File Offset: 0x00148BE5
		public static void DrawOptionUnselected(Rect rect)
		{
			GUI.color = Widgets.OptionUnselectedBGFillColor;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = Widgets.OptionUnselectedBGBorderColor;
			Widgets.DrawBox(rect, 1, null);
			GUI.color = Color.white;
		}

		// Token: 0x06003374 RID: 13172 RVA: 0x0014AA18 File Offset: 0x00148C18
		public static void DrawOptionSelected(Rect rect)
		{
			GUI.color = Widgets.OptionSelectedBGFillColor;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = Widgets.OptionSelectedBGBorderColor;
			Widgets.DrawBox(rect.ExpandedBy(3f), 3, null);
			GUI.color = Color.white;
		}

		// Token: 0x06003375 RID: 13173 RVA: 0x0014AA55 File Offset: 0x00148C55
		public static void DrawOptionBackground(Rect rect, bool selected)
		{
			if (selected)
			{
				Widgets.DrawOptionSelected(rect);
			}
			else
			{
				Widgets.DrawOptionUnselected(rect);
			}
			Widgets.DrawHighlightIfMouseover(rect);
		}

		// Token: 0x06003376 RID: 13174 RVA: 0x0014AA70 File Offset: 0x00148C70
		public static void DrawShadowAround(Rect rect)
		{
			Rect rect2 = rect.ContractedBy(-9f);
			rect2.x += 2f;
			rect2.y += 2f;
			Widgets.DrawAtlas(rect2, Widgets.ShadowAtlas);
		}

		// Token: 0x06003377 RID: 13175 RVA: 0x0014AABA File Offset: 0x00148CBA
		public static void DrawAtlas(Rect rect, Texture2D atlas)
		{
			Widgets.DrawAtlas(rect, atlas, true);
		}

		// Token: 0x06003378 RID: 13176 RVA: 0x0014AAC4 File Offset: 0x00148CC4
		public static void DrawAtlas(Rect rect, Texture2D atlas, bool drawTop)
		{
			if (Event.current.type != EventType.repaint)
			{
				return;
			}
			rect.x = Mathf.Round(rect.x);
			rect.y = Mathf.Round(rect.y);
			rect.width = Mathf.Round(rect.width);
			rect.height = Mathf.Round(rect.height);
			rect = UIScaling.AdjustRectToUIScaling(rect);
			float num = (float)atlas.width * 0.25f;
			num = UIScaling.AdjustCoordToUIScalingCeil(GenMath.Min(num, rect.height / 2f, rect.width / 2f));
			Rect drawRect;
			if (drawTop)
			{
				drawRect = new Rect(rect.x, rect.y, num, num);
				Widgets.DrawTexturePart(drawRect, Widgets.AtlasUV_TopLeft, atlas);
				drawRect = new Rect(rect.x + rect.width - num, rect.y, num, num);
				Widgets.DrawTexturePart(drawRect, Widgets.AtlasUV_TopRight, atlas);
			}
			drawRect = new Rect(rect.x, rect.y + rect.height - num, num, num);
			Widgets.DrawTexturePart(drawRect, Widgets.AtlasUV_BottomLeft, atlas);
			drawRect = new Rect(rect.x + rect.width - num, rect.y + rect.height - num, num, num);
			Widgets.DrawTexturePart(drawRect, Widgets.AtlasUV_BottomRight, atlas);
			drawRect = new Rect(rect.x + num, rect.y + num, rect.width - num * 2f, rect.height - num * 2f);
			if (!drawTop)
			{
				drawRect.height += num;
				drawRect.y -= num;
			}
			Widgets.DrawTexturePart(drawRect, Widgets.AtlasUV_Center, atlas);
			if (drawTop)
			{
				drawRect = new Rect(rect.x + num, rect.y, rect.width - num * 2f, num);
				Widgets.DrawTexturePart(drawRect, Widgets.AtlasUV_Top, atlas);
			}
			drawRect = new Rect(rect.x + num, rect.y + rect.height - num, rect.width - num * 2f, num);
			Widgets.DrawTexturePart(drawRect, Widgets.AtlasUV_Bottom, atlas);
			drawRect = new Rect(rect.x, rect.y + num, num, rect.height - num * 2f);
			if (!drawTop)
			{
				drawRect.height += num;
				drawRect.y -= num;
			}
			Widgets.DrawTexturePart(drawRect, Widgets.AtlasUV_Left, atlas);
			drawRect = new Rect(rect.x + rect.width - num, rect.y + num, num, rect.height - num * 2f);
			if (!drawTop)
			{
				drawRect.height += num;
				drawRect.y -= num;
			}
			Widgets.DrawTexturePart(drawRect, Widgets.AtlasUV_Right, atlas);
		}

		// Token: 0x06003379 RID: 13177 RVA: 0x0014ADA8 File Offset: 0x00148FA8
		public static void DrawAtlasWithMaterial(Rect rect, Texture2D atlas, Material mat, bool drawTop = true)
		{
			rect.x = Mathf.Round(rect.x);
			rect.y = Mathf.Round(rect.y);
			rect.width = Mathf.Round(rect.width);
			rect.height = Mathf.Round(rect.height);
			rect = UIScaling.AdjustRectToUIScaling(rect);
			float num = (float)atlas.width * 0.25f;
			num = UIScaling.AdjustCoordToUIScalingCeil(GenMath.Min(num, rect.height / 2f, rect.width / 2f));
			Widgets.BeginGroup(rect);
			Rect rect2;
			if (drawTop)
			{
				rect2 = new Rect(0f, 0f, num, num);
				GenUI.DrawTexturePartWithMaterial(rect2, atlas, mat, Widgets.AtlasUV_TopLeft);
				rect2 = new Rect(rect.width - num, 0f, num, num);
				GenUI.DrawTexturePartWithMaterial(rect2, atlas, mat, Widgets.AtlasUV_TopRight);
			}
			rect2 = new Rect(0f, rect.height - num, num, num);
			GenUI.DrawTexturePartWithMaterial(rect2, atlas, mat, Widgets.AtlasUV_BottomLeft);
			rect2 = new Rect(rect.width - num, rect.height - num, num, num);
			GenUI.DrawTexturePartWithMaterial(rect2, atlas, mat, Widgets.AtlasUV_BottomRight);
			rect2 = new Rect(num, num, rect.width - num * 2f, rect.height - num * 2f);
			if (!drawTop)
			{
				rect2.height += num;
				rect2.y -= num;
			}
			GenUI.DrawTexturePartWithMaterial(rect2, atlas, mat, Widgets.AtlasUV_Center);
			if (drawTop)
			{
				rect2 = new Rect(num, 0f, rect.width - num * 2f, num);
				GenUI.DrawTexturePartWithMaterial(rect2, atlas, mat, Widgets.AtlasUV_Top);
			}
			rect2 = new Rect(num, rect.height - num, rect.width - num * 2f, num);
			GenUI.DrawTexturePartWithMaterial(rect2, atlas, mat, Widgets.AtlasUV_Bottom);
			rect2 = new Rect(0f, num, num, rect.height - num * 2f);
			if (!drawTop)
			{
				rect2.height += num;
				rect2.y -= num;
			}
			GenUI.DrawTexturePartWithMaterial(rect2, atlas, mat, Widgets.AtlasUV_Left);
			rect2 = new Rect(rect.width - num, num, num, rect.height - num * 2f);
			if (!drawTop)
			{
				rect2.height += num;
				rect2.y -= num;
			}
			GenUI.DrawTexturePartWithMaterial(rect2, atlas, mat, Widgets.AtlasUV_Right);
			Widgets.EndGroup();
		}

		// Token: 0x0600337A RID: 13178 RVA: 0x0014B025 File Offset: 0x00149225
		public static Rect ToUVRect(this Rect r, Vector2 texSize)
		{
			return new Rect(r.x / texSize.x, r.y / texSize.y, r.width / texSize.x, r.height / texSize.y);
		}

		// Token: 0x0600337B RID: 13179 RVA: 0x0014B064 File Offset: 0x00149264
		public static void DrawTexturePart(Rect drawRect, Rect uvRect, Texture2D tex)
		{
			uvRect.y = 1f - uvRect.y - uvRect.height;
			GUI.DrawTextureWithTexCoords(drawRect, tex, uvRect);
		}

		// Token: 0x0600337C RID: 13180 RVA: 0x0014B08C File Offset: 0x0014928C
		public static void ScrollHorizontal(Rect outRect, ref Vector2 scrollPosition, Rect viewRect, float ScrollWheelSpeed = 20f)
		{
			if (Event.current.type == EventType.scrollWheel && Mouse.IsOver(outRect))
			{
				scrollPosition.x += Event.current.delta.y * ScrollWheelSpeed;
				float num = 0f;
				float num2 = viewRect.width - outRect.width + 16f;
				if (scrollPosition.x < num)
				{
					scrollPosition.x = num;
				}
				if (scrollPosition.x > num2)
				{
					scrollPosition.x = num2;
				}
				Event.current.Use();
			}
		}

		// Token: 0x0600337D RID: 13181 RVA: 0x0014B110 File Offset: 0x00149310
		public static void BeginScrollView(Rect outRect, ref Vector2 scrollPosition, Rect viewRect, bool showScrollbars = true)
		{
			if (Widgets.mouseOverScrollViewStack.Count > 0)
			{
				Widgets.mouseOverScrollViewStack.Push(Widgets.mouseOverScrollViewStack.Peek() && outRect.Contains(Event.current.mousePosition));
			}
			else
			{
				Widgets.mouseOverScrollViewStack.Push(outRect.Contains(Event.current.mousePosition));
			}
			SteamDeck.HandleTouchScreenScrollViewScroll(outRect, ref scrollPosition);
			if (showScrollbars)
			{
				scrollPosition = GUI.BeginScrollView(outRect, scrollPosition, viewRect);
			}
			else
			{
				scrollPosition = GUI.BeginScrollView(outRect, scrollPosition, viewRect, GUIStyle.none, GUIStyle.none);
			}
			UnityGUIBugsFixer.Notify_BeginScrollView();
		}

		// Token: 0x0600337E RID: 13182 RVA: 0x0014B1B4 File Offset: 0x001493B4
		public static void AdjustRectsForScrollView(Rect parentRect, ref Rect outRect, ref Rect viewRect)
		{
			if (viewRect.height >= outRect.height)
			{
				viewRect.width -= 20f;
				outRect.xMax -= 4f;
				outRect.yMin = Mathf.Max(parentRect.yMin + 6f, outRect.yMin);
				outRect.yMax = Mathf.Min(parentRect.yMax - 6f, outRect.yMax);
			}
		}

		// Token: 0x0600337F RID: 13183 RVA: 0x0014B22F File Offset: 0x0014942F
		public static void EndScrollView()
		{
			Widgets.mouseOverScrollViewStack.Pop();
			GUI.EndScrollView();
		}

		// Token: 0x06003380 RID: 13184 RVA: 0x0014B241 File Offset: 0x00149441
		public static void EnsureMousePositionStackEmpty()
		{
			if (Widgets.mouseOverScrollViewStack.Count > 0)
			{
				Log.Error("Mouse position stack is not empty. There were more calls to BeginScrollView than EndScrollView. Fixing.");
				Widgets.mouseOverScrollViewStack.Clear();
			}
		}

		// Token: 0x06003381 RID: 13185 RVA: 0x0014B264 File Offset: 0x00149464
		public static void ColorSelectorIcon(Rect rect, Texture icon, Color color, bool drawColor = false)
		{
			if (icon != null)
			{
				GUI.color = color;
				GUI.DrawTexture(rect, icon);
				GUI.color = Color.white;
				return;
			}
			if (drawColor)
			{
				Widgets.DrawBoxSolid(rect, color);
			}
		}

		// Token: 0x06003382 RID: 13186 RVA: 0x0014B294 File Offset: 0x00149494
		public static bool ColorBox(Rect rect, ref Color color, Color boxColor, int colorSize = 22, int colorPadding = 2, Action<Color, Rect> extraOnGUI = null)
		{
			Widgets.DrawLightHighlight(rect);
			Widgets.DrawHighlightIfMouseover(rect);
			if (color.IndistinguishableFrom(boxColor))
			{
				Widgets.DrawBox(rect, 1, null);
			}
			Rect rect2 = new Rect(rect.x + (float)colorPadding, rect.y + (float)colorPadding, (float)colorSize, (float)colorSize);
			Widgets.DrawBoxSolid(rect2, boxColor);
			if (extraOnGUI != null)
			{
				extraOnGUI(boxColor, rect2);
			}
			bool result = false;
			if (Widgets.ButtonInvisible(rect, true))
			{
				result = true;
				color = boxColor;
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			}
			return result;
		}

		// Token: 0x06003383 RID: 13187 RVA: 0x0014B318 File Offset: 0x00149518
		public static bool ColorSelector(Rect rect, ref Color color, List<Color> colors, out float height, Texture icon = null, int colorSize = 22, int colorPadding = 2, Action<Color, Rect> extraOnGUI = null)
		{
			height = 0f;
			bool result = false;
			int num = colorSize + colorPadding * 2;
			float num2 = (icon != null) ? ((float)(colorSize * 4) + 10f) : 0f;
			int num3 = Mathf.FloorToInt((rect.width - num2 + (float)colorPadding) / (float)(num + colorPadding));
			int num4 = Mathf.CeilToInt((float)colors.Count / (float)num3);
			Widgets.BeginGroup(rect);
			Widgets.ColorSelectorIcon(new Rect(5f, 5f, (float)(colorSize * 4), (float)(colorSize * 4)), icon, color, false);
			for (int i = 0; i < colors.Count; i++)
			{
				int num5 = i / num3;
				int num6 = i % num3;
				float num7 = (icon != null) ? ((num2 - (float)(num * num4) - (float)colorPadding) / 2f) : 0f;
				Rect rect2 = new Rect(num2 + (float)(num6 * num) + (float)(num6 * colorPadding), num7 + (float)(num5 * num) + (float)(num5 * colorPadding), (float)num, (float)num);
				if (Widgets.ColorBox(rect2, ref color, colors[i], colorSize, colorPadding, extraOnGUI))
				{
					result = true;
				}
				height = Mathf.Max(height, rect2.yMax);
			}
			Widgets.EndGroup();
			return result;
		}

		// Token: 0x06003384 RID: 13188 RVA: 0x0014B450 File Offset: 0x00149650
		private static void DrawColorSelectionCircle(Rect hsvColorWheelRect, Vector2Int center, Color color)
		{
			int num = (int)Mathf.Round(hsvColorWheelRect.width * 0.125f);
			GUI.DrawTexture(new Rect((float)(center.x - num / 2), (float)(center.y - num / 2), (float)num, (float)num), Widgets.ColorSelectionCircle, ScaleMode.ScaleToFit, true, 1f, color, 0f, 0f);
		}

		// Token: 0x06003385 RID: 13189 RVA: 0x0014B4AE File Offset: 0x001496AE
		private static bool ClickedInsideRect(Rect rect)
		{
			return Event.current.type == EventType.mouseDown && rect.Contains(Event.current.mousePosition);
		}

		// Token: 0x06003386 RID: 13190 RVA: 0x0014B4D0 File Offset: 0x001496D0
		public static void HSVColorWheel(Rect rect, ref Color color, ref bool currentlyDragging, float? colorValueOverride = null, string controlName = null)
		{
			if (rect.width != rect.height)
			{
				throw new ArgumentException("HSV color wheel must be drawn in a square rect.");
			}
			float num;
			float d;
			float defaultValue;
			Color.RGBToHSV(color, out num, out d, out defaultValue);
			float valueOrDefault = colorValueOverride.GetValueOrDefault(defaultValue);
			GUI.DrawTexture(rect, Widgets.HSVColorWheelTex, ScaleMode.ScaleToFit, true, 1f, Color.HSVToRGB(0f, 0f, valueOrDefault), 0f, 0f);
			num = (num + 0.25f) * 2f * 3.1415927f;
			Vector2 a = new Vector2(Mathf.Cos(num), -Mathf.Sin(num)) * d * rect.width / 2f;
			Widgets.DrawColorSelectionCircle(rect, Vector2Int.RoundToInt(a + rect.center), (valueOrDefault > 0.5f) ? Color.black : Color.white);
			if (!currentlyDragging)
			{
				MouseoverSounds.DoRegion(rect);
			}
			if (Event.current.isMouse && Event.current.button == 0)
			{
				if (currentlyDragging && Event.current.type == EventType.mouseUp)
				{
					currentlyDragging = false;
					return;
				}
				if (Widgets.ClickedInsideRect(rect) | currentlyDragging)
				{
					GUI.FocusControl(controlName);
					currentlyDragging = true;
					Vector2 vector = (Event.current.mousePosition - rect.center) / (rect.size / 2f);
					float num2 = Mathf.Atan2(-vector.y, vector.x) / 6.2831855f;
					num2 += 1.75f;
					num2 %= 1f;
					float s = Mathf.Clamp01(vector.magnitude);
					color = Color.HSVToRGB(num2, s, valueOrDefault);
					Event.current.Use();
				}
			}
		}

		// Token: 0x06003387 RID: 13191 RVA: 0x0014B68C File Offset: 0x0014988C
		public static void ColorTemperatureBar(Rect rect, ref Color color, ref bool dragging, float? colorValueOverride = null)
		{
			float num = colorValueOverride ?? Mathf.Max(new float[]
			{
				color.r,
				color.g,
				color.b
			});
			float? num2 = color.ColorTemperature();
			string label = ((num2 != null) ? num2.GetValueOrDefault().ToString("0.K") : null) ?? "";
			RectDivider rect2 = new RectDivider(rect, 661493905, new Vector2?(new Vector2(17f, 0f)));
			using (new TextBlock(TextAnchor.MiddleLeft))
			{
				string text = "ColorTemperature".Translate().CapitalizeFirst();
				Widgets.Label(rect2.NewCol(Text.CalcSize(text).x, HorizontalJustification.Left, null), text);
				Widgets.Label(rect2.NewCol(Text.CalcSize("XXXXXK").x, HorizontalJustification.Left, null), label);
			}
			if (!dragging)
			{
				TooltipHandler.TipRegion(rect, "ColorTemperatureTooltip".Translate());
				MouseoverSounds.DoRegion(rect);
			}
			if (Event.current.button == 0)
			{
				if (dragging && Event.current.type == EventType.mouseUp)
				{
					dragging = false;
				}
				else if (Widgets.ClickedInsideRect(rect2) || (dragging && UnityGUIBugsFixer.MouseDrag(0)))
				{
					dragging = true;
					if (Event.current.type == EventType.mouseDrag)
					{
						Event.current.Use();
					}
					float fraction = Mathf.Clamp01((Event.current.mousePosition.x - rect2.Rect.xMin) / rect2.Rect.width);
					num2 = new float?(GenMath.ExponentialWarpInterpolation(1000f, 40000f, fraction, new Vector2(0.5f, 6600f)));
					color = GenColor.FromColorTemperature(num2.Value);
					color *= num;
				}
			}
			rect2.NewRow(6f, VerticalJustification.Top, null);
			rect2.NewRow(6f, VerticalJustification.Bottom, null);
			GUI.DrawTexture(rect2, Widgets.ColorTemperatureExp, ScaleMode.StretchToFill, true, 1f, Color.HSVToRGB(0f, 0f, num), 0f, 0f);
			if (num2 != null)
			{
				float num3 = rect2.Rect.width * GenMath.InverseExponentialWarpInterpolation(1000f, 40000f, num2.Value, new Vector2(0.5f, 6600f));
				Rect position = new Rect(rect2.Rect.x + num3 - 6f, rect2.Rect.y - 6f, 12f, 12f);
				Rect position2 = new Rect(rect2.Rect.x + num3 - 6f, rect2.Rect.yMax - 6f, 12f, 12f);
				GUI.DrawTextureWithTexCoords(position, Widgets.SelectionArrow, new Rect(0f, 1f, 1f, -1f), true);
				GUI.DrawTextureWithTexCoords(position2, Widgets.SelectionArrow, new Rect(0f, 0f, 1f, 1f), true);
			}
		}

		// Token: 0x06003388 RID: 13192 RVA: 0x0014BA38 File Offset: 0x00149C38
		private static int ToIntegerRange(float fraction, int min, int max)
		{
			return Mathf.Clamp(Mathf.RoundToInt(fraction * (float)max), min, max);
		}

		// Token: 0x06003389 RID: 13193 RVA: 0x0014BA4C File Offset: 0x00149C4C
		public static bool ColorTextfields(ref RectAggregator aggregator, ref Color color, ref string[] buffers, ref Color colorBuffer, string previousFocusedControlName, string controlName = null, Widgets.ColorComponents editable = Widgets.ColorComponents.All, Widgets.ColorComponents visible = Widgets.ColorComponents.All)
		{
			if (visible == Widgets.ColorComponents.None)
			{
				return false;
			}
			if ((~(visible != Widgets.ColorComponents.None) & editable) != Widgets.ColorComponents.None)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Cannot have editable but invisible components ");
				defaultInterpolatedStringHandler.AppendFormatted<Widgets.ColorComponents>(~visible & editable);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			string text;
			if ((text = controlName) == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(15, 2);
				defaultInterpolatedStringHandler2.AppendLiteral("ColorTextfields");
				defaultInterpolatedStringHandler2.AppendFormatted<float>(aggregator.Rect.x);
				defaultInterpolatedStringHandler2.AppendFormatted<float>(aggregator.Rect.y);
				text = defaultInterpolatedStringHandler2.ToStringAndClear();
			}
			controlName = text;
			bool flag = previousFocusedControlName != null && previousFocusedControlName.StartsWith(controlName);
			bool flag2 = GUI.GetNameOfFocusedControl().StartsWith(controlName);
			using (new TextBlock(TextAnchor.MiddleLeft))
			{
				float num = 30f;
				float num2 = 0f;
				for (int i = 0; i < Widgets.colorComponentLabels.Length; i++)
				{
					Widgets.tmpTranslatedColorComponentLabels[i] = Widgets.colorComponentLabels[i].Translate().CapitalizeFirst();
					num = Mathf.Max(num, Widgets.tmpTranslatedColorComponentLabels[i].GetHeightCached());
					num2 = Mathf.Max(num2, Widgets.tmpTranslatedColorComponentLabels[i].GetWidthCached());
				}
				float fraction;
				float fraction2;
				float fraction3;
				Color.RGBToHSV(colorBuffer, out fraction, out fraction2, out fraction3);
				Widgets.intColorComponents[0] = Widgets.ToIntegerRange(colorBuffer.r, 0, Widgets.maxColorComponentValues[0]);
				Widgets.intColorComponents[1] = Widgets.ToIntegerRange(colorBuffer.g, 0, Widgets.maxColorComponentValues[1]);
				Widgets.intColorComponents[2] = Widgets.ToIntegerRange(colorBuffer.b, 0, Widgets.maxColorComponentValues[2]);
				Widgets.intColorComponents[3] = Widgets.ToIntegerRange(fraction, 0, Widgets.maxColorComponentValues[3]);
				Widgets.intColorComponents[4] = Widgets.ToIntegerRange(fraction2, 0, Widgets.maxColorComponentValues[4]);
				Widgets.intColorComponents[5] = Widgets.ToIntegerRange(fraction3, 0, Widgets.maxColorComponentValues[5]);
				for (int j = 0; j <= 5; j++)
				{
					Widgets.ColorComponents colorComponents = (Widgets.ColorComponents)(1 << j);
					if ((visible & colorComponents) != Widgets.ColorComponents.None)
					{
						RectDivider rect = aggregator.NewRow(num, VerticalJustification.Bottom);
						Widgets.Label(rect.NewCol(num2, HorizontalJustification.Left, null), Widgets.tmpTranslatedColorComponentLabels[j]);
						if ((editable & colorComponents) == Widgets.ColorComponents.None)
						{
							Widgets.Label(rect, Widgets.intColorComponents[j].ToString());
						}
						else
						{
							string text2 = Widgets.intColorComponents[j].ToString();
							Rect rect2 = rect;
							string text3 = text2;
							string[] array = buffers;
							int num3 = j;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(1, 2);
							defaultInterpolatedStringHandler3.AppendFormatted(controlName);
							defaultInterpolatedStringHandler3.AppendLiteral("_");
							defaultInterpolatedStringHandler3.AppendFormatted<int>(j);
							string text4 = Widgets.DelayedTextField(rect2, text3, ref array[num3], previousFocusedControlName, defaultInterpolatedStringHandler3.ToStringAndClear());
							int num4;
							if (text2 != text4 && int.TryParse(text4, out num4))
							{
								Widgets.intColorComponents[j] = num4;
								if (j < 3)
								{
									colorBuffer = new ColorInt(Widgets.intColorComponents[0], Widgets.intColorComponents[1], Widgets.intColorComponents[2]).ToColor;
								}
								else
								{
									colorBuffer = Color.HSVToRGB((float)Widgets.intColorComponents[3] / 360f, (float)Widgets.intColorComponents[4] / 100f, (float)Widgets.intColorComponents[5] / 100f);
								}
							}
						}
					}
				}
			}
			if (flag)
			{
				if (!flag2)
				{
					color = colorBuffer;
					return true;
				}
			}
			else
			{
				colorBuffer = color;
			}
			return false;
		}

		// Token: 0x0600338A RID: 13194 RVA: 0x0014BDE0 File Offset: 0x00149FE0
		public static void DrawHighlightSelected(Rect rect)
		{
			GUI.DrawTexture(rect, TexUI.HighlightSelectedTex);
		}

		// Token: 0x0600338B RID: 13195 RVA: 0x0014BDED File Offset: 0x00149FED
		public static void DrawHighlightIfMouseover(Rect rect)
		{
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
		}

		// Token: 0x0600338C RID: 13196 RVA: 0x0014BDFD File Offset: 0x00149FFD
		public static void DrawHighlight(Rect rect)
		{
			GUI.DrawTexture(rect, TexUI.HighlightTex);
		}

		// Token: 0x0600338D RID: 13197 RVA: 0x0014BE0A File Offset: 0x0014A00A
		public static void DrawHighlight(Rect rect, float opacity)
		{
			GUI.color = Color.white.ToTransparent(opacity);
			GUI.DrawTexture(rect, TexUI.HighlightTex);
			GUI.color = Color.white;
		}

		// Token: 0x0600338E RID: 13198 RVA: 0x0014BE31 File Offset: 0x0014A031
		public static void DrawLightHighlight(Rect rect)
		{
			GUI.DrawTexture(rect, Widgets.LightHighlight);
		}

		// Token: 0x0600338F RID: 13199 RVA: 0x0014BE3E File Offset: 0x0014A03E
		public static void DrawStrongHighlight(Rect rect, Color? color = null)
		{
			Color color2 = GUI.color;
			GUI.color = color.GetValueOrDefault(Widgets.HighlightStrongBgColor);
			Widgets.DrawAtlas(rect, TexUI.RectHighlight);
			GUI.color = color2;
		}

		// Token: 0x06003390 RID: 13200 RVA: 0x0014BE68 File Offset: 0x0014A068
		public static void DrawTextHighlight(Rect rect, float expandBy = 4f, Color? color = null)
		{
			rect.y -= expandBy;
			rect.height += expandBy * 2f;
			Color color2 = GUI.color;
			GUI.color = color.GetValueOrDefault(Widgets.HighlightTextBgColor);
			Widgets.DrawAtlas(rect, TexUI.RectHighlight);
			GUI.color = color2;
		}

		// Token: 0x06003391 RID: 13201 RVA: 0x0014BEBF File Offset: 0x0014A0BF
		public static void DrawTitleBG(Rect rect)
		{
			GUI.DrawTexture(rect, TexUI.TitleBGTex);
		}

		// Token: 0x06003392 RID: 13202 RVA: 0x0014BECC File Offset: 0x0014A0CC
		public static bool InfoCardButton(float x, float y, Thing thing)
		{
			IConstructible constructible = thing as IConstructible;
			if (constructible != null)
			{
				ThingDef thingDef = thing.def.entityDefToBuild as ThingDef;
				if (thingDef != null)
				{
					return Widgets.InfoCardButton(x, y, thingDef, constructible.EntityToBuildStuff());
				}
				return Widgets.InfoCardButton(x, y, thing.def.entityDefToBuild);
			}
			else
			{
				if (Widgets.InfoCardButtonWorker(x, y))
				{
					Find.WindowStack.Add(new Dialog_InfoCard(thing, null));
					return true;
				}
				return false;
			}
		}

		// Token: 0x06003393 RID: 13203 RVA: 0x0014BF36 File Offset: 0x0014A136
		public static bool InfoCardButton(float x, float y, Def def)
		{
			if (Widgets.InfoCardButtonWorker(x, y))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(def, null));
				return true;
			}
			return false;
		}

		// Token: 0x06003394 RID: 13204 RVA: 0x0014BF55 File Offset: 0x0014A155
		public static bool InfoCardButton(Rect rect, Def def)
		{
			if (Widgets.InfoCardButtonWorker(rect))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(def, null));
				return true;
			}
			return false;
		}

		// Token: 0x06003395 RID: 13205 RVA: 0x0014BF73 File Offset: 0x0014A173
		public static bool InfoCardButton(float x, float y, Def def, Precept_ThingStyle precept)
		{
			if (Widgets.InfoCardButtonWorker(x, y))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(def, precept));
				return true;
			}
			return false;
		}

		// Token: 0x06003396 RID: 13206 RVA: 0x0014BF92 File Offset: 0x0014A192
		public static bool InfoCardButton(float x, float y, ThingDef thingDef, ThingDef stuffDef)
		{
			if (Widgets.InfoCardButtonWorker(x, y))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(thingDef, stuffDef, null));
				return true;
			}
			return false;
		}

		// Token: 0x06003397 RID: 13207 RVA: 0x0014BFB2 File Offset: 0x0014A1B2
		public static bool InfoCardButton(float x, float y, WorldObject worldObject)
		{
			if (Widgets.InfoCardButtonWorker(x, y))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(worldObject));
				return true;
			}
			return false;
		}

		// Token: 0x06003398 RID: 13208 RVA: 0x0014BFD0 File Offset: 0x0014A1D0
		public static bool InfoCardButton(Rect rect, Hediff hediff)
		{
			if (Widgets.InfoCardButtonWorker(rect))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(hediff));
				return true;
			}
			return false;
		}

		// Token: 0x06003399 RID: 13209 RVA: 0x0014BFED File Offset: 0x0014A1ED
		public static bool InfoCardButton(float x, float y, Faction faction)
		{
			if (Widgets.InfoCardButtonWorker(x, y))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(faction));
				return true;
			}
			return false;
		}

		// Token: 0x0600339A RID: 13210 RVA: 0x0014C00B File Offset: 0x0014A20B
		public static bool InfoCardButtonCentered(Rect rect, Thing thing)
		{
			return Widgets.InfoCardButton(rect.center.x - 12f, rect.center.y - 12f, thing);
		}

		// Token: 0x0600339B RID: 13211 RVA: 0x0014C037 File Offset: 0x0014A237
		public static bool InfoCardButtonCentered(Rect rect, Faction faction)
		{
			return Widgets.InfoCardButton(rect.center.x - 12f, rect.center.y - 12f, faction);
		}

		// Token: 0x0600339C RID: 13212 RVA: 0x0014C063 File Offset: 0x0014A263
		private static bool InfoCardButtonWorker(float x, float y)
		{
			return Widgets.InfoCardButtonWorker(new Rect(x, y, 24f, 24f));
		}

		// Token: 0x0600339D RID: 13213 RVA: 0x0014C07B File Offset: 0x0014A27B
		private static bool InfoCardButtonWorker(Rect rect)
		{
			MouseoverSounds.DoRegion(rect);
			TooltipHandler.TipRegionByKey(rect, "DefInfoTip");
			bool result = Widgets.ButtonImage(rect, TexButton.Info, GUI.color, true, null);
			UIHighlighter.HighlightOpportunity(rect, "InfoCard");
			return result;
		}

		// Token: 0x0600339E RID: 13214 RVA: 0x0014C0AB File Offset: 0x0014A2AB
		public static void DrawTextureFitted(Rect outerRect, Texture tex, float scale, float alpha = 1f)
		{
			Widgets.DrawTextureFitted(outerRect, tex, scale, new Vector2((float)tex.width, (float)tex.height), new Rect(0f, 0f, 1f, 1f), 0f, null, alpha);
		}

		// Token: 0x0600339F RID: 13215 RVA: 0x0014C0E8 File Offset: 0x0014A2E8
		public static void DrawTextureFitted(Rect outerRect, Texture tex, float scale, Material mat, float alpha = 1f)
		{
			Widgets.DrawTextureFitted(outerRect, tex, scale, new Vector2((float)tex.width, (float)tex.height), new Rect(0f, 0f, 1f, 1f), 0f, mat, alpha);
		}

		// Token: 0x060033A0 RID: 13216 RVA: 0x0014C128 File Offset: 0x0014A328
		public static void DrawTextureFitted(Rect outerRect, Texture tex, float scale, Vector2 texProportions, Rect texCoords, float angle = 0f, Material mat = null, float alpha = 1f)
		{
			if (Event.current.type != EventType.repaint)
			{
				return;
			}
			Rect rect = new Rect(0f, 0f, texProportions.x, texProportions.y);
			float num;
			if (rect.width / rect.height < outerRect.width / outerRect.height)
			{
				num = outerRect.height / rect.height;
			}
			else
			{
				num = outerRect.width / rect.width;
			}
			num *= scale;
			rect.width *= num;
			rect.height *= num;
			rect.x = outerRect.x + outerRect.width / 2f - rect.width / 2f;
			rect.y = outerRect.y + outerRect.height / 2f - rect.height / 2f;
			Matrix4x4 matrix = Matrix4x4.identity;
			if (angle != 0f)
			{
				matrix = GUI.matrix;
				UI.RotateAroundPivot(angle, rect.center);
			}
			Color color = Color.white;
			if (!Mathf.Approximately(alpha, 1f))
			{
				Color color2;
				color = (color2 = GUI.color);
				color2.a *= alpha;
				GUI.color = color2;
			}
			GenUI.DrawTextureWithMaterial(rect, tex, mat, texCoords);
			if (angle != 0f)
			{
				GUI.matrix = matrix;
			}
			if (!Mathf.Approximately(alpha, 1f))
			{
				GUI.color = color;
			}
		}

		// Token: 0x060033A1 RID: 13217 RVA: 0x0014C29C File Offset: 0x0014A49C
		public static void DrawTextureRotated(Vector2 center, Texture tex, float angle, float scale = 1f, Material material = null)
		{
			float num = (float)tex.width * scale;
			float num2 = (float)tex.height * scale;
			Widgets.DrawTextureRotated(new Rect(center.x - num / 2f, center.y - num2 / 2f, num, num2), tex, angle, material);
		}

		// Token: 0x060033A2 RID: 13218 RVA: 0x0014C2EC File Offset: 0x0014A4EC
		public static void DrawTextureRotated(Rect rect, Texture tex, float angle, Material material = null)
		{
			if (Event.current.type != EventType.repaint)
			{
				return;
			}
			if (angle != 0f)
			{
				Matrix4x4 matrix = GUI.matrix;
				UI.RotateAroundPivot(angle, rect.center);
				if (material == null)
				{
					GUI.DrawTexture(rect, tex);
				}
				else
				{
					GenUI.DrawTextureWithMaterial(rect, tex, material, default(Rect));
				}
				GUI.matrix = matrix;
				return;
			}
			if (material == null)
			{
				GUI.DrawTexture(rect, tex);
				return;
			}
			GenUI.DrawTextureWithMaterial(rect, tex, material, default(Rect));
		}

		// Token: 0x060033A3 RID: 13219 RVA: 0x0014C36C File Offset: 0x0014A56C
		public static void NoneLabel(float y, float width, string customLabel = null)
		{
			Widgets.NoneLabel(ref y, width, customLabel);
		}

		// Token: 0x060033A4 RID: 13220 RVA: 0x0014C378 File Offset: 0x0014A578
		public static void NoneLabel(ref float curY, float width, string customLabel = null)
		{
			GUI.color = Color.gray;
			Text.Anchor = TextAnchor.UpperCenter;
			Widgets.Label(new Rect(0f, curY, width, 30f), customLabel ?? "NoneBrackets".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			curY += 25f;
			GUI.color = Color.white;
		}

		// Token: 0x060033A5 RID: 13221 RVA: 0x0014C3DA File Offset: 0x0014A5DA
		public static void NoneLabelCenteredVertically(Rect rect, string customLabel = null)
		{
			GUI.color = Color.gray;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, customLabel ?? "NoneBrackets".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
		}

		// Token: 0x060033A6 RID: 13222 RVA: 0x0014C418 File Offset: 0x0014A618
		public static void DraggableBar(Rect barRect, Texture2D barTexture, Texture2D barHighlightTexture, Texture2D emptyBarTex, Texture2D dragBarTex, ref bool draggingBar, float barValue, ref float targetValue, IEnumerable<float> bandPercentages = null, int increments = 20, float min = 0f, float max = 1f)
		{
			bool flag = Mouse.IsOver(barRect);
			Widgets.FillableBar(barRect, Mathf.Min(barValue, 1f), flag ? barHighlightTexture : barTexture, emptyBarTex, true);
			if (bandPercentages != null)
			{
				foreach (float percent in bandPercentages)
				{
					Widgets.DrawDraggableBarThreshold(barRect, percent, barValue);
				}
			}
			float num = Mathf.Clamp(Mathf.Round((Event.current.mousePosition.x - barRect.x) / barRect.width * (float)increments) / (float)increments, min, max);
			Event current = Event.current;
			if (current.type == EventType.mouseDown && current.button == 0 && flag)
			{
				targetValue = num;
				draggingBar = true;
				current.Use();
			}
			if ((UnityGUIBugsFixer.MouseDrag(0) & draggingBar) && flag)
			{
				if (Math.Abs(num - targetValue) > 1E-45f)
				{
					SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				}
				targetValue = num;
				if (Event.current.type == EventType.mouseDrag)
				{
					current.Use();
				}
			}
			if ((current.type == EventType.mouseUp && current.button == 0) & draggingBar)
			{
				draggingBar = false;
				current.Use();
			}
			Widgets.DrawDraggableBarTarget(barRect, draggingBar ? num : targetValue, dragBarTex);
			GUI.color = Color.white;
		}

		// Token: 0x060033A7 RID: 13223 RVA: 0x0014C570 File Offset: 0x0014A770
		private static void DrawDraggableBarThreshold(Rect rect, float percent, float curValue)
		{
			Rect position = new Rect
			{
				x = rect.x + 3f + (rect.width - 8f) * percent,
				y = rect.y + rect.height - 9f,
				width = 2f,
				height = 6f
			};
			if (curValue < percent)
			{
				GUI.DrawTexture(position, BaseContent.GreyTex);
				return;
			}
			GUI.DrawTexture(position, BaseContent.BlackTex);
		}

		// Token: 0x060033A8 RID: 13224 RVA: 0x0014C5FC File Offset: 0x0014A7FC
		private static void DrawDraggableBarTarget(Rect rect, float percent, Texture2D targetTex)
		{
			float num = Mathf.Round((rect.width - 8f) * percent);
			GUI.DrawTexture(new Rect
			{
				x = rect.x + 3f + num,
				y = rect.y,
				width = 2f,
				height = rect.height
			}, targetTex);
			float num2 = UIScaling.AdjustCoordToUIScalingFloor(rect.x + 2f + num);
			float xMax = UIScaling.AdjustCoordToUIScalingCeil(num2 + 4f);
			Rect rect2 = new Rect
			{
				y = rect.y - 3f,
				height = 5f,
				xMin = num2,
				xMax = xMax
			};
			GUI.DrawTexture(rect2, targetTex);
			Rect position = rect2;
			position.y = rect.yMax - 2f;
			GUI.DrawTexture(position, targetTex);
		}

		// Token: 0x060033A9 RID: 13225 RVA: 0x0014C6EC File Offset: 0x0014A8EC
		public static void Dropdown<Target, Payload>(Rect rect, Target target, Func<Target, Payload> getPayload, Func<Target, IEnumerable<Widgets.DropdownMenuElement<Payload>>> menuGenerator, string buttonLabel = null, Texture2D buttonIcon = null, string dragLabel = null, Texture2D dragIcon = null, Action dropdownOpened = null, bool paintable = false)
		{
			Widgets.Dropdown<Target, Payload>(rect, target, Color.white, getPayload, menuGenerator, buttonLabel, buttonIcon, dragLabel, dragIcon, dropdownOpened, paintable, null);
		}

		// Token: 0x060033AA RID: 13226 RVA: 0x0014C71C File Offset: 0x0014A91C
		public static void Dropdown<Target, Payload>(Rect rect, Target target, Color iconColor, Func<Target, Payload> getPayload, Func<Target, IEnumerable<Widgets.DropdownMenuElement<Payload>>> menuGenerator, string buttonLabel = null, Texture2D buttonIcon = null, string dragLabel = null, Texture2D dragIcon = null, Action dropdownOpened = null, bool paintable = false, float? contractButtonIcon = null)
		{
			MouseoverSounds.DoRegion(rect);
			Widgets.DraggableResult draggableResult;
			if (buttonIcon != null)
			{
				Widgets.DrawHighlightIfMouseover(rect);
				GUI.color = iconColor;
				Rect rect2 = rect;
				if (contractButtonIcon != null)
				{
					rect2 = rect2.ContractedBy(contractButtonIcon.Value);
				}
				Widgets.DrawTextureFitted(rect2, buttonIcon, 1f, 1f);
				GUI.color = Color.white;
				draggableResult = Widgets.ButtonInvisibleDraggable(rect, false);
			}
			else
			{
				draggableResult = Widgets.ButtonTextDraggable(rect, buttonLabel, true, false, true, null);
			}
			if (draggableResult == Widgets.DraggableResult.Pressed)
			{
				List<FloatMenuOption> options = (from opt in menuGenerator(target)
				select opt.option).ToList<FloatMenuOption>();
				Find.WindowStack.Add(new FloatMenu(options));
				if (dropdownOpened != null)
				{
					dropdownOpened();
					return;
				}
			}
			else
			{
				if (paintable && draggableResult == Widgets.DraggableResult.Dragged)
				{
					Widgets.dropdownPainting = true;
					Widgets.dropdownPainting_Payload = getPayload(target);
					Widgets.dropdownPainting_Type = typeof(Payload);
					Widgets.dropdownPainting_Text = ((dragLabel != null) ? dragLabel : buttonLabel);
					Widgets.dropdownPainting_Icon = ((dragIcon != null) ? dragIcon : buttonIcon);
					return;
				}
				if (paintable && Widgets.dropdownPainting && Mouse.IsOver(rect) && Widgets.dropdownPainting_Type == typeof(Payload))
				{
					FloatMenuOption floatMenuOption = (from opt in menuGenerator(target)
					where object.Equals(opt.payload, Widgets.dropdownPainting_Payload)
					select opt.option).FirstOrDefault<FloatMenuOption>();
					if (floatMenuOption != null && !floatMenuOption.Disabled)
					{
						Payload x = getPayload(target);
						floatMenuOption.action();
						Payload y = getPayload(target);
						if (!EqualityComparer<Payload>.Default.Equals(x, y))
						{
							SoundDefOf.Click.PlayOneShotOnCamera(null);
						}
					}
				}
			}
		}

		// Token: 0x060033AB RID: 13227 RVA: 0x0014C913 File Offset: 0x0014AB13
		public static void MouseAttachedLabel(string label, float xOffset = 0f, float yOffset = 0f, Color? colorOverride = null)
		{
			Rect rect = Widgets.CreateMouseAttachedLabelRect(label, xOffset, yOffset);
			if (colorOverride != null)
			{
				GUI.color = colorOverride.Value;
			}
			Widgets.Label(rect, label);
			GUI.color = Color.white;
		}

		// Token: 0x060033AC RID: 13228 RVA: 0x0014C942 File Offset: 0x0014AB42
		public static void MouseAttachedLabel(TaggedString label, float xOffset = 0f, float yOffset = 0f, Color? colorOverride = null)
		{
			Rect rect = Widgets.CreateMouseAttachedLabelRect(label, xOffset, yOffset);
			if (colorOverride != null)
			{
				GUI.color = colorOverride.Value;
			}
			Widgets.Label(rect, label);
			GUI.color = Color.white;
		}

		// Token: 0x060033AD RID: 13229 RVA: 0x0014C978 File Offset: 0x0014AB78
		public static void WorldAttachedLabel(Vector3 worldPos, string label, float xOffset = 0f, float yOffset = 0f, Color? colorOverride = null)
		{
			Vector3 vector = Find.WorldCamera.WorldToScreenPoint(worldPos);
			vector.y = (float)Screen.height - vector.y;
			vector /= Prefs.UIScale;
			Rect rect = Widgets.CreateAttachedLabelRect(vector, label, xOffset, yOffset);
			if (colorOverride != null)
			{
				GUI.color = colorOverride.Value;
			}
			Widgets.Label(rect, label);
			GUI.color = Color.white;
		}

		// Token: 0x060033AE RID: 13230 RVA: 0x0014C9E4 File Offset: 0x0014ABE4
		private static Rect CreateMouseAttachedLabelRect(string label, float xOffset, float yOffset)
		{
			return Widgets.CreateAttachedLabelRect(Event.current.mousePosition, label, xOffset, yOffset);
		}

		// Token: 0x060033AF RID: 13231 RVA: 0x0014C9F8 File Offset: 0x0014ABF8
		private static Rect CreateAttachedLabelRect(Vector2 screenPosition, string label, float xOffset, float yOffset)
		{
			Rect rect = new Rect(screenPosition.x + 8f + xOffset, screenPosition.y + 8f + yOffset, 32f, 32f);
			GUI.color = Color.white;
			Text.Font = GameFont.Small;
			Rect result = new Rect(rect.xMax, rect.y, 9999f, 100f);
			Vector2 vector = Text.CalcSize(label);
			result.height = Mathf.Max(result.height, vector.y);
			GUI.DrawTexture(new Rect(result.x - vector.x * 0.1f, result.y, vector.x * 1.2f, vector.y), TexUI.GrayTextBG);
			return result;
		}

		// Token: 0x060033B0 RID: 13232 RVA: 0x0014CAC0 File Offset: 0x0014ACC0
		public static void WidgetsOnGUI()
		{
			if (Event.current.rawType == EventType.mouseUp || Input.GetMouseButtonUp(0))
			{
				Widgets.checkboxPainting = false;
				Widgets.dropdownPainting = false;
			}
			if (Widgets.checkboxPainting)
			{
				GenUI.DrawMouseAttachment(Widgets.checkboxPaintingState ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex);
			}
			if (Widgets.dropdownPainting)
			{
				GenUI.DrawMouseAttachment(Widgets.dropdownPainting_Icon, Widgets.dropdownPainting_Text, 0f, default(Vector2), null, null, false, default(Color), null, null);
			}
		}

		// Token: 0x04002128 RID: 8488
		public static Stack<bool> mouseOverScrollViewStack = new Stack<bool>();

		// Token: 0x04002129 RID: 8489
		public static readonly GUIStyle EmptyStyle = new GUIStyle();

		// Token: 0x0400212A RID: 8490
		[TweakValue("Input", 0f, 100f)]
		private static float DragStartDistanceSquared = 20f;

		// Token: 0x0400212B RID: 8491
		public const int LeftMouseButton = 0;

		// Token: 0x0400212C RID: 8492
		public static readonly Color InactiveColor = new Color(0.37f, 0.37f, 0.37f, 0.8f);

		// Token: 0x0400212D RID: 8493
		public static readonly Color HighlightStrongBgColor = ColorLibrary.SkyBlue;

		// Token: 0x0400212E RID: 8494
		public static readonly Color HighlightTextBgColor = Widgets.HighlightStrongBgColor.ToTransparent(0.25f);

		// Token: 0x0400212F RID: 8495
		private static readonly Texture2D DefaultBarBgTex = BaseContent.BlackTex;

		// Token: 0x04002130 RID: 8496
		public static readonly Texture2D BarFullTexHor = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.8f, 0.85f));

		// Token: 0x04002131 RID: 8497
		public static readonly Texture2D CheckboxOnTex = ContentFinder<Texture2D>.Get("UI/Widgets/CheckOn", true);

		// Token: 0x04002132 RID: 8498
		public static readonly Texture2D CheckboxOffTex = ContentFinder<Texture2D>.Get("UI/Widgets/CheckOff", true);

		// Token: 0x04002133 RID: 8499
		public static readonly Texture2D CheckboxPartialTex = ContentFinder<Texture2D>.Get("UI/Widgets/CheckPartial", true);

		// Token: 0x04002134 RID: 8500
		public const float CheckboxSize = 24f;

		// Token: 0x04002135 RID: 8501
		public const float RadioButtonSize = 24f;

		// Token: 0x04002136 RID: 8502
		public static readonly Texture2D RadioButOnTex = ContentFinder<Texture2D>.Get("UI/Widgets/RadioButOn", true);

		// Token: 0x04002137 RID: 8503
		public static readonly Texture2D HSVColorWheelTex = ContentFinder<Texture2D>.Get("UI/Widgets/HSVColorWheel", true);

		// Token: 0x04002138 RID: 8504
		public static readonly Texture2D ColorSelectionCircle = ContentFinder<Texture2D>.Get("UI/Overlays/TargetHighlight_Square", true);

		// Token: 0x04002139 RID: 8505
		public static readonly Texture2D ColorTemperatureExp = ContentFinder<Texture2D>.Get("UI/Widgets/ColorTemperatureExp", true);

		// Token: 0x0400213A RID: 8506
		public static readonly Texture2D SelectionArrow = ContentFinder<Texture2D>.Get("Things/Mote/InteractionArrow", true);

		// Token: 0x0400213B RID: 8507
		private static readonly Texture2D RadioButOffTex = ContentFinder<Texture2D>.Get("UI/Widgets/RadioButOff", true);

		// Token: 0x0400213C RID: 8508
		private static readonly Texture2D FillArrowTexRight = ContentFinder<Texture2D>.Get("UI/Widgets/FillChangeArrowRight", true);

		// Token: 0x0400213D RID: 8509
		private static readonly Texture2D FillArrowTexLeft = ContentFinder<Texture2D>.Get("UI/Widgets/FillChangeArrowLeft", true);

		// Token: 0x0400213E RID: 8510
		public static readonly Texture2D PlaceholderIconTex = ContentFinder<Texture2D>.Get("UI/Icons/MenuOptionNoIcon", true);

		// Token: 0x0400213F RID: 8511
		private const int FillableBarBorderWidth = 3;

		// Token: 0x04002140 RID: 8512
		private const int MaxFillChangeArrowHeight = 16;

		// Token: 0x04002141 RID: 8513
		private const int FillChangeArrowWidth = 8;

		// Token: 0x04002142 RID: 8514
		public const float CloseButtonSize = 18f;

		// Token: 0x04002143 RID: 8515
		public const float CloseButtonMargin = 4f;

		// Token: 0x04002144 RID: 8516
		public const float BackButtonWidth = 120f;

		// Token: 0x04002145 RID: 8517
		public const float BackButtonHeight = 40f;

		// Token: 0x04002146 RID: 8518
		public const float BackButtonMargin = 16f;

		// Token: 0x04002147 RID: 8519
		private const float ColorHighlightCircleFraction = 0.125f;

		// Token: 0x04002148 RID: 8520
		private const float ColorTextfieldHeight = 30f;

		// Token: 0x04002149 RID: 8521
		private const float SelectionArrowSize = 12f;

		// Token: 0x0400214A RID: 8522
		private static readonly Texture2D ShadowAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/DropShadow", true);

		// Token: 0x0400214B RID: 8523
		public static readonly Texture2D ButtonBGAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/ButtonBG", true);

		// Token: 0x0400214C RID: 8524
		private static readonly Texture2D ButtonBGAtlasMouseover = ContentFinder<Texture2D>.Get("UI/Widgets/ButtonBGMouseover", true);

		// Token: 0x0400214D RID: 8525
		public static readonly Texture2D ButtonBGAtlasClick = ContentFinder<Texture2D>.Get("UI/Widgets/ButtonBGClick", true);

		// Token: 0x0400214E RID: 8526
		private static readonly Texture2D FloatRangeSliderTex = ContentFinder<Texture2D>.Get("UI/Widgets/RangeSlider", true);

		// Token: 0x0400214F RID: 8527
		public static readonly Texture2D LightHighlight = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.04f));

		// Token: 0x04002150 RID: 8528
		private static readonly Rect DefaultTexCoords = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x04002151 RID: 8529
		private static readonly Rect LinkedTexCoords = new Rect(0f, 0.5f, 0.25f, 0.25f);

		// Token: 0x04002152 RID: 8530
		[TweakValue("Input", 0f, 100f)]
		private static int IntEntryButtonWidth = 40;

		// Token: 0x04002153 RID: 8531
		private static Texture2D LineTexAA = null;

		// Token: 0x04002154 RID: 8532
		private static readonly Texture2D AltTexture = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.05f));

		// Token: 0x04002155 RID: 8533
		public static readonly Color NormalOptionColor = new Color(0.8f, 0.85f, 1f);

		// Token: 0x04002156 RID: 8534
		public static readonly Color MouseoverOptionColor = Color.yellow;

		// Token: 0x04002157 RID: 8535
		private static Dictionary<string, float> LabelCache = new Dictionary<string, float>();

		// Token: 0x04002158 RID: 8536
		private const float TileSize = 64f;

		// Token: 0x04002159 RID: 8537
		public static readonly Color SeparatorLabelColor = new Color(0.8f, 0.8f, 0.8f, 1f);

		// Token: 0x0400215A RID: 8538
		public static readonly Color SeparatorLineColor = new Color(0.3f, 0.3f, 0.3f, 1f);

		// Token: 0x0400215B RID: 8539
		private const float SeparatorLabelHeight = 20f;

		// Token: 0x0400215C RID: 8540
		public const float ListSeparatorHeight = 25f;

		// Token: 0x0400215D RID: 8541
		private static bool checkboxPainting = false;

		// Token: 0x0400215E RID: 8542
		private static bool checkboxPaintingState = false;

		// Token: 0x0400215F RID: 8543
		public static readonly Texture2D ButtonSubtleAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/ButtonSubtleAtlas", true);

		// Token: 0x04002160 RID: 8544
		private static readonly Texture2D SliderRailAtlas = ContentFinder<Texture2D>.Get("UI/Buttons/SliderRail", true);

		// Token: 0x04002161 RID: 8545
		private static readonly Texture2D SliderHandle = ContentFinder<Texture2D>.Get("UI/Buttons/SliderHandle", true);

		// Token: 0x04002162 RID: 8546
		private static readonly Texture2D ButtonBarTex = SolidColorMaterials.NewSolidColorTexture(TexUI.FinishedResearchColorTransparent);

		// Token: 0x04002163 RID: 8547
		public const float ButtonSubtleDefaultMarginPct = 0.15f;

		// Token: 0x04002164 RID: 8548
		private static int buttonInvisibleDraggable_activeControl = 0;

		// Token: 0x04002165 RID: 8549
		private static bool buttonInvisibleDraggable_dragged = false;

		// Token: 0x04002166 RID: 8550
		private static Vector3 buttonInvisibleDraggable_mouseStart = Vector3.zero;

		// Token: 0x04002167 RID: 8551
		private static int sliderDraggingID;

		// Token: 0x04002168 RID: 8552
		private const float SliderHandleSize = 12f;

		// Token: 0x04002169 RID: 8553
		public const float RangeControlIdealHeight = 31f;

		// Token: 0x0400216A RID: 8554
		public const float RangeControlCompactHeight = 32f;

		// Token: 0x0400216B RID: 8555
		private const float RangeSliderSize = 16f;

		// Token: 0x0400216C RID: 8556
		private static readonly Color RangeControlTextColor = new Color(0.6f, 0.6f, 0.6f);

		// Token: 0x0400216D RID: 8557
		private static int draggingId = 0;

		// Token: 0x0400216E RID: 8558
		private static Widgets.RangeEnd curDragEnd = Widgets.RangeEnd.None;

		// Token: 0x0400216F RID: 8559
		private static float lastDragSliderSoundTime = -1f;

		// Token: 0x04002170 RID: 8560
		private static float FillableBarChangeRateDisplayRatio = 100000000f;

		// Token: 0x04002171 RID: 8561
		public static int MaxFillableBarChangeRate = 3;

		// Token: 0x04002172 RID: 8562
		private static readonly Color WindowBGBorderColor = new ColorInt(97, 108, 122).ToColor;

		// Token: 0x04002173 RID: 8563
		public static readonly Color WindowBGFillColor = new ColorInt(21, 25, 29).ToColor;

		// Token: 0x04002174 RID: 8564
		public static readonly Color MenuSectionBGFillColor = new ColorInt(42, 43, 44).ToColor;

		// Token: 0x04002175 RID: 8565
		private static readonly Color MenuSectionBGBorderColor = new ColorInt(135, 135, 135).ToColor;

		// Token: 0x04002176 RID: 8566
		private static readonly Color TutorWindowBGFillColor = new ColorInt(133, 85, 44).ToColor;

		// Token: 0x04002177 RID: 8567
		private static readonly Color TutorWindowBGBorderColor = new ColorInt(176, 139, 61).ToColor;

		// Token: 0x04002178 RID: 8568
		private static readonly Color OptionUnselectedBGFillColor = new Color(0.21f, 0.21f, 0.21f);

		// Token: 0x04002179 RID: 8569
		private static readonly Color OptionUnselectedBGBorderColor = Widgets.OptionUnselectedBGFillColor * 1.8f;

		// Token: 0x0400217A RID: 8570
		private static readonly Color OptionSelectedBGFillColor = new Color(0.32f, 0.28f, 0.21f);

		// Token: 0x0400217B RID: 8571
		private static readonly Color OptionSelectedBGBorderColor = Widgets.OptionSelectedBGFillColor * 1.8f;

		// Token: 0x0400217C RID: 8572
		private static readonly Rect AtlasUV_TopLeft = new Rect(0f, 0f, 0.25f, 0.25f);

		// Token: 0x0400217D RID: 8573
		private static readonly Rect AtlasUV_TopRight = new Rect(0.75f, 0f, 0.25f, 0.25f);

		// Token: 0x0400217E RID: 8574
		private static readonly Rect AtlasUV_BottomLeft = new Rect(0f, 0.75f, 0.25f, 0.25f);

		// Token: 0x0400217F RID: 8575
		private static readonly Rect AtlasUV_BottomRight = new Rect(0.75f, 0.75f, 0.25f, 0.25f);

		// Token: 0x04002180 RID: 8576
		private static readonly Rect AtlasUV_Top = new Rect(0.25f, 0f, 0.5f, 0.25f);

		// Token: 0x04002181 RID: 8577
		private static readonly Rect AtlasUV_Bottom = new Rect(0.25f, 0.75f, 0.5f, 0.25f);

		// Token: 0x04002182 RID: 8578
		private static readonly Rect AtlasUV_Left = new Rect(0f, 0.25f, 0.25f, 0.5f);

		// Token: 0x04002183 RID: 8579
		private static readonly Rect AtlasUV_Right = new Rect(0.75f, 0.25f, 0.25f, 0.5f);

		// Token: 0x04002184 RID: 8580
		private static readonly Rect AtlasUV_Center = new Rect(0.25f, 0.25f, 0.5f, 0.5f);

		// Token: 0x04002185 RID: 8581
		private static int[] maxColorComponentValues = new int[]
		{
			255,
			255,
			255,
			360,
			100,
			100
		};

		// Token: 0x04002186 RID: 8582
		private static string[] colorComponentLabels = new string[]
		{
			"Red",
			"Green",
			"Blue",
			"Hue",
			"Saturation",
			"ColorValue"
		};

		// Token: 0x04002187 RID: 8583
		private static string[] tmpTranslatedColorComponentLabels = new string[6];

		// Token: 0x04002188 RID: 8584
		private static int[] intColorComponents = new int[6];

		// Token: 0x04002189 RID: 8585
		public const float InfoCardButtonSize = 24f;

		// Token: 0x0400218A RID: 8586
		private static bool dropdownPainting = false;

		// Token: 0x0400218B RID: 8587
		private static object dropdownPainting_Payload = null;

		// Token: 0x0400218C RID: 8588
		private static Type dropdownPainting_Type = null;

		// Token: 0x0400218D RID: 8589
		private static string dropdownPainting_Text = "";

		// Token: 0x0400218E RID: 8590
		private static Texture2D dropdownPainting_Icon = null;

		// Token: 0x020029F4 RID: 10740
		public enum DraggableResult
		{
			// Token: 0x0400AC15 RID: 44053
			Idle,
			// Token: 0x0400AC16 RID: 44054
			Pressed,
			// Token: 0x0400AC17 RID: 44055
			Dragged,
			// Token: 0x0400AC18 RID: 44056
			DraggedThenPressed
		}

		// Token: 0x020029F5 RID: 10741
		private enum RangeEnd : byte
		{
			// Token: 0x0400AC1A RID: 44058
			None,
			// Token: 0x0400AC1B RID: 44059
			Min,
			// Token: 0x0400AC1C RID: 44060
			Max
		}

		// Token: 0x020029F6 RID: 10742
		[Flags]
		public enum ColorComponents
		{
			// Token: 0x0400AC1E RID: 44062
			Red = 1,
			// Token: 0x0400AC1F RID: 44063
			Green = 2,
			// Token: 0x0400AC20 RID: 44064
			Blue = 4,
			// Token: 0x0400AC21 RID: 44065
			Hue = 8,
			// Token: 0x0400AC22 RID: 44066
			Sat = 16,
			// Token: 0x0400AC23 RID: 44067
			Value = 32,
			// Token: 0x0400AC24 RID: 44068
			None = 0,
			// Token: 0x0400AC25 RID: 44069
			All = 63
		}

		// Token: 0x020029F7 RID: 10743
		public struct DropdownMenuElement<Payload>
		{
			// Token: 0x0400AC26 RID: 44070
			public FloatMenuOption option;

			// Token: 0x0400AC27 RID: 44071
			public Payload payload;
		}
	}
}

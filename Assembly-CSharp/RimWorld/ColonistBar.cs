using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C27 RID: 7207
	[StaticConstructorOnStartup]
	public class ColonistBar
	{
		// Token: 0x1700212C RID: 8492
		// (get) Token: 0x0600B7C5 RID: 47045 RVA: 0x0040CB81 File Offset: 0x0040AD81
		public List<ColonistBar.Entry> Entries
		{
			get
			{
				this.CheckRecacheEntries();
				return this.cachedEntries;
			}
		}

		// Token: 0x1700212D RID: 8493
		// (get) Token: 0x0600B7C6 RID: 47046 RVA: 0x0040CB90 File Offset: 0x0040AD90
		private bool ShowGroupFrames
		{
			get
			{
				List<ColonistBar.Entry> entries = this.Entries;
				int num = -1;
				for (int i = 0; i < entries.Count; i++)
				{
					num = Mathf.Max(num, entries[i].group);
				}
				return num >= 1;
			}
		}

		// Token: 0x1700212E RID: 8494
		// (get) Token: 0x0600B7C7 RID: 47047 RVA: 0x0040CBD1 File Offset: 0x0040ADD1
		public float Scale
		{
			get
			{
				return this.cachedScale;
			}
		}

		// Token: 0x1700212F RID: 8495
		// (get) Token: 0x0600B7C8 RID: 47048 RVA: 0x0040CBD9 File Offset: 0x0040ADD9
		public List<Vector2> DrawLocs
		{
			get
			{
				return this.cachedDrawLocs;
			}
		}

		// Token: 0x17002130 RID: 8496
		// (get) Token: 0x0600B7C9 RID: 47049 RVA: 0x0040CBE1 File Offset: 0x0040ADE1
		public Vector2 Size
		{
			get
			{
				return ColonistBar.BaseSize * this.Scale;
			}
		}

		// Token: 0x17002131 RID: 8497
		// (get) Token: 0x0600B7CA RID: 47050 RVA: 0x0040CBF3 File Offset: 0x0040ADF3
		public float SpaceBetweenColonistsHorizontal
		{
			get
			{
				return 24f * this.Scale;
			}
		}

		// Token: 0x17002132 RID: 8498
		// (get) Token: 0x0600B7CB RID: 47051 RVA: 0x0040CC01 File Offset: 0x0040AE01
		private bool Visible
		{
			get
			{
				return UI.screenWidth >= 800 && UI.screenHeight >= 500 && !Find.TilePicker.Active;
			}
		}

		// Token: 0x0600B7CC RID: 47052 RVA: 0x0040CC2C File Offset: 0x0040AE2C
		public void MarkColonistsDirty()
		{
			this.entriesDirty = true;
		}

		// Token: 0x0600B7CD RID: 47053 RVA: 0x0040CC38 File Offset: 0x0040AE38
		public void ColonistBarOnGUI()
		{
			if (!this.Visible)
			{
				return;
			}
			if (Event.current.type != EventType.layout)
			{
				List<ColonistBar.Entry> entries = this.Entries;
				int num = -1;
				bool showGroupFrames = this.ShowGroupFrames;
				int value = -1;
				for (int i = 0; i < this.cachedDrawLocs.Count; i++)
				{
					Rect rect = new Rect(this.cachedDrawLocs[i].x, this.cachedDrawLocs[i].y, this.Size.x, this.Size.y);
					ColonistBar.Entry entry = entries[i];
					bool flag = num != entry.group;
					num = entry.group;
					if (Event.current.type == EventType.repaint)
					{
						if (flag)
						{
							value = ReorderableWidget.NewGroup(entry.reorderAction, ReorderableDirection.Horizontal, new Rect(0f, 0f, (float)UI.screenWidth, (float)UI.screenHeight), this.SpaceBetweenColonistsHorizontal, entry.extraDraggedItemOnGUI, true);
						}
						this.cachedReorderableGroups[i] = value;
					}
					bool reordering;
					if (entry.pawn != null)
					{
						this.drawer.HandleClicks(rect, entry.pawn, this.cachedReorderableGroups[i], out reordering);
					}
					else
					{
						reordering = false;
					}
					if (Event.current.type == EventType.repaint)
					{
						if (flag && showGroupFrames)
						{
							this.drawer.DrawGroupFrame(entry.group);
						}
						if (entry.pawn != null)
						{
							this.drawer.DrawColonist(rect, entry.pawn, entry.map, this.colonistsToHighlight.Contains(entry.pawn), reordering);
							Pawn_EquipmentTracker equipment = entry.pawn.equipment;
							ThingWithComps thingWithComps = (equipment != null) ? equipment.Primary : null;
							if ((Prefs.ShowWeaponsUnderPortraitMode == ShowWeaponsUnderPortraitMode.Always || (Prefs.ShowWeaponsUnderPortraitMode == ShowWeaponsUnderPortraitMode.WhileDrafted && entry.pawn.Drafted)) && thingWithComps != null && thingWithComps.def.IsWeapon)
							{
								Widgets.ThingIcon(new Rect(rect.x, rect.y + rect.height * 1.05f, rect.width, rect.height).ScaledBy(0.75f), thingWithComps, 1f, null, true, 1f, false);
							}
						}
					}
				}
				num = -1;
				if (showGroupFrames)
				{
					for (int j = 0; j < this.cachedDrawLocs.Count; j++)
					{
						ColonistBar.Entry entry2 = entries[j];
						bool flag2 = num != entry2.group;
						num = entry2.group;
						if (flag2)
						{
							this.drawer.HandleGroupFrameClicks(entry2.group);
						}
					}
				}
			}
			if (Event.current.type == EventType.repaint)
			{
				this.colonistsToHighlight.Clear();
			}
		}

		// Token: 0x0600B7CE RID: 47054 RVA: 0x0040CEEC File Offset: 0x0040B0EC
		private void CheckRecacheEntries()
		{
			if (!this.entriesDirty)
			{
				return;
			}
			this.entriesDirty = false;
			this.cachedEntries.Clear();
			int num = 0;
			if (Find.PlaySettings.showColonistBar)
			{
				ColonistBar.tmpMaps.Clear();
				ColonistBar.tmpMaps.AddRange(Find.Maps);
				ColonistBar.tmpMaps.SortBy((Map x) => !x.IsPlayerHome, (Map x) => x.uniqueID);
				for (int i = 0; i < ColonistBar.tmpMaps.Count; i++)
				{
					ColonistBar.tmpPawns.Clear();
					ColonistBar.tmpPawns.AddRange(ColonistBar.tmpMaps[i].mapPawns.FreeColonists);
					ColonistBar.tmpPawns.AddRange(ColonistBar.tmpMaps[i].mapPawns.ColonySubhumansControllable);
					List<Thing> list = ColonistBar.tmpMaps[i].listerThings.ThingsInGroup(ThingRequestGroup.Corpse);
					for (int j = 0; j < list.Count; j++)
					{
						if (!list[j].IsDessicated())
						{
							Pawn innerPawn = ((Corpse)list[j]).InnerPawn;
							if (innerPawn != null && innerPawn.IsColonist)
							{
								ColonistBar.tmpPawns.Add(innerPawn);
							}
						}
					}
					IReadOnlyList<Pawn> allPawnsSpawned = ColonistBar.tmpMaps[i].mapPawns.AllPawnsSpawned;
					for (int k = 0; k < allPawnsSpawned.Count; k++)
					{
						Corpse corpse = allPawnsSpawned[k].carryTracker.CarriedThing as Corpse;
						if (corpse != null && !corpse.IsDessicated() && corpse.InnerPawn.IsColonist)
						{
							ColonistBar.tmpPawns.Add(corpse.InnerPawn);
						}
					}
					foreach (Pawn pawn in ColonistBar.tmpPawns)
					{
						if (pawn.playerSettings.displayOrder == -9999999)
						{
							pawn.playerSettings.displayOrder = Mathf.Max(ColonistBar.tmpPawns.MaxBy((Pawn p) => p.playerSettings.displayOrder).playerSettings.displayOrder, 0) + 1;
						}
					}
					PlayerPawnsDisplayOrderUtility.Sort(ColonistBar.tmpPawns);
					foreach (Pawn pawn2 in ColonistBar.tmpPawns)
					{
						this.cachedEntries.Add(new ColonistBar.Entry(pawn2, ColonistBar.tmpMaps[i], num));
					}
					if (!ColonistBar.tmpPawns.Any<Pawn>())
					{
						this.cachedEntries.Add(new ColonistBar.Entry(null, ColonistBar.tmpMaps[i], num));
					}
					num++;
				}
				ColonistBar.tmpCaravans.Clear();
				ColonistBar.tmpCaravans.AddRange(Find.WorldObjects.Caravans);
				ColonistBar.tmpCaravans.SortBy((Caravan x) => x.ID);
				for (int l = 0; l < ColonistBar.tmpCaravans.Count; l++)
				{
					if (ColonistBar.tmpCaravans[l].IsPlayerControlled)
					{
						ColonistBar.tmpPawns.Clear();
						ColonistBar.tmpPawns.AddRange(ColonistBar.tmpCaravans[l].PawnsListForReading);
						PlayerPawnsDisplayOrderUtility.Sort(ColonistBar.tmpPawns);
						for (int m = 0; m < ColonistBar.tmpPawns.Count; m++)
						{
							if (ColonistBar.tmpPawns[m].IsColonist || ColonistBar.tmpPawns[m].IsColonySubhumanPlayerControlled)
							{
								this.cachedEntries.Add(new ColonistBar.Entry(ColonistBar.tmpPawns[m], null, num));
							}
						}
						num++;
					}
				}
			}
			this.cachedReorderableGroups.Clear();
			foreach (ColonistBar.Entry entry in this.cachedEntries)
			{
				this.cachedReorderableGroups.Add(-1);
			}
			this.drawer.Notify_RecachedEntries();
			ColonistBar.tmpPawns.Clear();
			ColonistBar.tmpMaps.Clear();
			ColonistBar.tmpCaravans.Clear();
			this.drawLocsFinder.CalculateDrawLocs(this.cachedDrawLocs, out this.cachedScale, num);
		}

		// Token: 0x0600B7CF RID: 47055 RVA: 0x0040D38C File Offset: 0x0040B58C
		public float GetEntryRectAlpha(Rect rect)
		{
			float t;
			if (Messages.CollidesWithAnyMessage(rect, out t))
			{
				return Mathf.Lerp(1f, 0.2f, t);
			}
			return 1f;
		}

		// Token: 0x0600B7D0 RID: 47056 RVA: 0x0040D3B9 File Offset: 0x0040B5B9
		public void Highlight(Pawn pawn)
		{
			if (!this.Visible)
			{
				return;
			}
			if (!this.colonistsToHighlight.Contains(pawn))
			{
				this.colonistsToHighlight.Add(pawn);
			}
		}

		// Token: 0x0600B7D1 RID: 47057 RVA: 0x0040D3E0 File Offset: 0x0040B5E0
		public void Reorder(int from, int to, int entryGroup)
		{
			int num = 0;
			Pawn pawn = null;
			Pawn pawn2 = null;
			Pawn pawn3 = null;
			for (int i = 0; i < this.cachedEntries.Count; i++)
			{
				if (this.cachedEntries[i].group == entryGroup && this.cachedEntries[i].pawn != null)
				{
					if (num == from)
					{
						pawn = this.cachedEntries[i].pawn;
					}
					if (num == to)
					{
						pawn2 = this.cachedEntries[i].pawn;
					}
					pawn3 = this.cachedEntries[i].pawn;
					num++;
				}
			}
			if (pawn == null)
			{
				return;
			}
			int num2 = (pawn2 != null) ? pawn2.playerSettings.displayOrder : (pawn3.playerSettings.displayOrder + 1);
			for (int j = 0; j < this.cachedEntries.Count; j++)
			{
				Pawn pawn4 = this.cachedEntries[j].pawn;
				if (pawn4 != null)
				{
					if (pawn4.playerSettings.displayOrder == num2)
					{
						if (pawn2 != null && this.cachedEntries[j].group == entryGroup)
						{
							pawn4.playerSettings.displayOrder++;
						}
					}
					else if (pawn4.playerSettings.displayOrder > num2)
					{
						pawn4.playerSettings.displayOrder++;
					}
					else
					{
						pawn4.playerSettings.displayOrder--;
					}
				}
			}
			pawn.playerSettings.displayOrder = num2;
			this.MarkColonistsDirty();
			MainTabWindowUtility.NotifyAllPawnTables_PawnsChanged();
		}

		// Token: 0x0600B7D2 RID: 47058 RVA: 0x0040D56C File Offset: 0x0040B76C
		public void DrawColonistMouseAttachment(int index, Vector2 dragStartPos, int entryGroup)
		{
			Pawn pawn = null;
			Vector2 vector = default(Vector2);
			int num = 0;
			for (int i = 0; i < this.cachedEntries.Count; i++)
			{
				if (this.cachedEntries[i].group == entryGroup && this.cachedEntries[i].pawn != null)
				{
					if (num == index)
					{
						pawn = this.cachedEntries[i].pawn;
						vector = this.cachedDrawLocs[i];
						break;
					}
					num++;
				}
			}
			if (pawn != null)
			{
				Texture iconTex = PortraitsCache.Get(pawn, ColonistBarColonistDrawer.PawnTextureSize, Rot4.South, ColonistBarColonistDrawer.PawnTextureCameraOffset, 1.28205f, true, true, true, true, null, null, false, null);
				Rect rect = new Rect(vector.x, vector.y, this.Size.x, this.Size.y);
				Rect pawnTextureRect = this.drawer.GetPawnTextureRect(rect.position);
				pawnTextureRect.position += Event.current.mousePosition - dragStartPos;
				string text = "";
				float angle = 0f;
				Rect? customRect = new Rect?(pawnTextureRect);
				GenUI.DrawMouseAttachment(iconTex, text, angle, default(Vector2), customRect, null, false, default(Color), null, null);
			}
		}

		// Token: 0x0600B7D3 RID: 47059 RVA: 0x0040D6C8 File Offset: 0x0040B8C8
		public bool AnyColonistOrCorpseAt(Vector2 pos)
		{
			ColonistBar.Entry entry;
			return this.TryGetEntryAt(pos, out entry) && entry.pawn != null;
		}

		// Token: 0x0600B7D4 RID: 47060 RVA: 0x0040D6EC File Offset: 0x0040B8EC
		public bool TryGetEntryAt(Vector2 pos, out ColonistBar.Entry entry)
		{
			List<Vector2> drawLocs = this.DrawLocs;
			List<ColonistBar.Entry> entries = this.Entries;
			Vector2 size = this.Size;
			for (int i = 0; i < drawLocs.Count; i++)
			{
				if (new Rect(drawLocs[i].x, drawLocs[i].y, size.x, size.y).Contains(pos))
				{
					entry = entries[i];
					return true;
				}
			}
			entry = default(ColonistBar.Entry);
			return false;
		}

		// Token: 0x0600B7D5 RID: 47061 RVA: 0x0040D76C File Offset: 0x0040B96C
		public List<Pawn> GetColonistsInOrder()
		{
			List<ColonistBar.Entry> entries = this.Entries;
			ColonistBar.tmpColonistsInOrder.Clear();
			for (int i = 0; i < entries.Count; i++)
			{
				if (entries[i].pawn != null)
				{
					ColonistBar.tmpColonistsInOrder.Add(entries[i].pawn);
				}
			}
			return ColonistBar.tmpColonistsInOrder;
		}

		// Token: 0x0600B7D6 RID: 47062 RVA: 0x0040D7C4 File Offset: 0x0040B9C4
		public List<Thing> ColonistsOrCorpsesInScreenRect(Rect rect)
		{
			List<Vector2> drawLocs = this.DrawLocs;
			List<ColonistBar.Entry> entries = this.Entries;
			Vector2 size = this.Size;
			ColonistBar.tmpColonistsWithMap.Clear();
			for (int i = 0; i < drawLocs.Count; i++)
			{
				if (rect.Overlaps(new Rect(drawLocs[i].x, drawLocs[i].y, size.x, size.y)))
				{
					Pawn pawn = entries[i].pawn;
					if (pawn != null)
					{
						Thing first;
						if (pawn.Dead && pawn.Corpse != null && pawn.Corpse.SpawnedOrAnyParentSpawned)
						{
							first = pawn.Corpse;
						}
						else
						{
							first = pawn;
						}
						ColonistBar.tmpColonistsWithMap.Add(new Pair<Thing, Map>(first, entries[i].map));
					}
				}
			}
			if (WorldRendererUtility.WorldSelected)
			{
				if (ColonistBar.tmpColonistsWithMap.Any((Pair<Thing, Map> x) => x.Second == null))
				{
					ColonistBar.tmpColonistsWithMap.RemoveAll((Pair<Thing, Map> x) => x.Second != null);
					goto IL_179;
				}
			}
			if (ColonistBar.tmpColonistsWithMap.Any((Pair<Thing, Map> x) => x.Second == Find.CurrentMap))
			{
				ColonistBar.tmpColonistsWithMap.RemoveAll((Pair<Thing, Map> x) => x.Second != Find.CurrentMap);
			}
			IL_179:
			ColonistBar.tmpColonists.Clear();
			for (int j = 0; j < ColonistBar.tmpColonistsWithMap.Count; j++)
			{
				ColonistBar.tmpColonists.Add(ColonistBar.tmpColonistsWithMap[j].First);
			}
			ColonistBar.tmpColonistsWithMap.Clear();
			return ColonistBar.tmpColonists;
		}

		// Token: 0x0600B7D7 RID: 47063 RVA: 0x0040D99C File Offset: 0x0040BB9C
		public List<Thing> MapColonistsOrCorpsesInScreenRect(Rect rect)
		{
			ColonistBar.tmpMapColonistsOrCorpsesInScreenRect.Clear();
			if (!this.Visible)
			{
				return ColonistBar.tmpMapColonistsOrCorpsesInScreenRect;
			}
			List<Thing> list = this.ColonistsOrCorpsesInScreenRect(rect);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Spawned)
				{
					ColonistBar.tmpMapColonistsOrCorpsesInScreenRect.Add(list[i]);
				}
			}
			return ColonistBar.tmpMapColonistsOrCorpsesInScreenRect;
		}

		// Token: 0x0600B7D8 RID: 47064 RVA: 0x0040DA00 File Offset: 0x0040BC00
		public List<Pawn> CaravanMembersInScreenRect(Rect rect)
		{
			ColonistBar.tmpCaravanPawns.Clear();
			if (!this.Visible)
			{
				return ColonistBar.tmpCaravanPawns;
			}
			List<Thing> list = this.ColonistsOrCorpsesInScreenRect(rect);
			for (int i = 0; i < list.Count; i++)
			{
				Pawn pawn = list[i] as Pawn;
				if (pawn != null && pawn.IsCaravanMember())
				{
					ColonistBar.tmpCaravanPawns.Add(pawn);
				}
			}
			return ColonistBar.tmpCaravanPawns;
		}

		// Token: 0x0600B7D9 RID: 47065 RVA: 0x0040DA68 File Offset: 0x0040BC68
		public List<Caravan> CaravanMembersCaravansInScreenRect(Rect rect)
		{
			ColonistBar.tmpCaravans.Clear();
			if (!this.Visible)
			{
				return ColonistBar.tmpCaravans;
			}
			List<Pawn> list = this.CaravanMembersInScreenRect(rect);
			for (int i = 0; i < list.Count; i++)
			{
				ColonistBar.tmpCaravans.Add(list[i].GetCaravan());
			}
			return ColonistBar.tmpCaravans;
		}

		// Token: 0x0600B7DA RID: 47066 RVA: 0x0040DAC4 File Offset: 0x0040BCC4
		public Caravan CaravanMemberCaravanAt(Vector2 at)
		{
			if (!this.Visible)
			{
				return null;
			}
			Pawn pawn = this.ColonistOrCorpseAt(at) as Pawn;
			if (pawn != null && pawn.IsCaravanMember())
			{
				return pawn.GetCaravan();
			}
			return null;
		}

		// Token: 0x0600B7DB RID: 47067 RVA: 0x0040DAFC File Offset: 0x0040BCFC
		public Thing ColonistOrCorpseAt(Vector2 pos)
		{
			if (!this.Visible)
			{
				return null;
			}
			ColonistBar.Entry entry;
			if (!this.TryGetEntryAt(pos, out entry))
			{
				return null;
			}
			Pawn pawn = entry.pawn;
			Thing result;
			if (pawn != null && pawn.Dead && pawn.Corpse != null && pawn.Corpse.SpawnedOrAnyParentSpawned)
			{
				result = pawn.Corpse;
			}
			else
			{
				result = pawn;
			}
			return result;
		}

		// Token: 0x04006954 RID: 26964
		public ColonistBarColonistDrawer drawer = new ColonistBarColonistDrawer();

		// Token: 0x04006955 RID: 26965
		private ColonistBarDrawLocsFinder drawLocsFinder = new ColonistBarDrawLocsFinder();

		// Token: 0x04006956 RID: 26966
		private List<ColonistBar.Entry> cachedEntries = new List<ColonistBar.Entry>();

		// Token: 0x04006957 RID: 26967
		private List<Vector2> cachedDrawLocs = new List<Vector2>();

		// Token: 0x04006958 RID: 26968
		private List<int> cachedReorderableGroups = new List<int>();

		// Token: 0x04006959 RID: 26969
		private float cachedScale = 1f;

		// Token: 0x0400695A RID: 26970
		private bool entriesDirty = true;

		// Token: 0x0400695B RID: 26971
		private List<Pawn> colonistsToHighlight = new List<Pawn>();

		// Token: 0x0400695C RID: 26972
		public static readonly Texture2D BGTex = Command.BGTex;

		// Token: 0x0400695D RID: 26973
		public static readonly Vector2 BaseSize = new Vector2(48f, 48f);

		// Token: 0x0400695E RID: 26974
		public const float BaseSelectedTexJump = 20f;

		// Token: 0x0400695F RID: 26975
		public const float BaseSelectedTexScale = 0.4f;

		// Token: 0x04006960 RID: 26976
		public const float EntryInAnotherMapAlpha = 0.4f;

		// Token: 0x04006961 RID: 26977
		public const float BaseSpaceBetweenGroups = 25f;

		// Token: 0x04006962 RID: 26978
		public const float BaseSpaceBetweenColonistsHorizontal = 24f;

		// Token: 0x04006963 RID: 26979
		public const float BaseSpaceBetweenColonistsVertical = 32f;

		// Token: 0x04006964 RID: 26980
		private const float WeaponIconOffsetScaleFactor = 1.05f;

		// Token: 0x04006965 RID: 26981
		private const float WeaponIconScaleFactor = 0.75f;

		// Token: 0x04006966 RID: 26982
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x04006967 RID: 26983
		private static List<Map> tmpMaps = new List<Map>();

		// Token: 0x04006968 RID: 26984
		private static List<Caravan> tmpCaravans = new List<Caravan>();

		// Token: 0x04006969 RID: 26985
		private static List<Pawn> tmpColonistsInOrder = new List<Pawn>();

		// Token: 0x0400696A RID: 26986
		private static List<Pair<Thing, Map>> tmpColonistsWithMap = new List<Pair<Thing, Map>>();

		// Token: 0x0400696B RID: 26987
		private static List<Thing> tmpColonists = new List<Thing>();

		// Token: 0x0400696C RID: 26988
		private static List<Thing> tmpMapColonistsOrCorpsesInScreenRect = new List<Thing>();

		// Token: 0x0400696D RID: 26989
		private static List<Pawn> tmpCaravanPawns = new List<Pawn>();

		// Token: 0x020038CC RID: 14540
		public struct Entry
		{
			// Token: 0x06014B4E RID: 84814 RVA: 0x006A0D5C File Offset: 0x0069EF5C
			public Entry(Pawn pawn, Map map, int group)
			{
				this.pawn = pawn;
				this.map = map;
				this.group = group;
				this.reorderAction = delegate(int from, int to)
				{
					Find.ColonistBar.Reorder(from, to, group);
				};
				this.extraDraggedItemOnGUI = delegate(int index, Vector2 dragStartPos)
				{
					Find.ColonistBar.DrawColonistMouseAttachment(index, dragStartPos, group);
				};
			}

			// Token: 0x0400E2CB RID: 58059
			public Pawn pawn;

			// Token: 0x0400E2CC RID: 58060
			public Map map;

			// Token: 0x0400E2CD RID: 58061
			public int group;

			// Token: 0x0400E2CE RID: 58062
			public Action<int, int> reorderAction;

			// Token: 0x0400E2CF RID: 58063
			public Action<int, Vector2> extraDraggedItemOnGUI;
		}
	}
}

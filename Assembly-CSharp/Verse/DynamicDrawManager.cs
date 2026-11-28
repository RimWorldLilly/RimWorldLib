using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Gilzoide.ManagedJobs;
using LudeonTK;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200025A RID: 602
	public sealed class DynamicDrawManager
	{
		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06001198 RID: 4504 RVA: 0x00064C9F File Offset: 0x00062E9F
		public IReadOnlyList<Thing> DrawThings
		{
			get
			{
				return this.drawThings;
			}
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x00064CA7 File Offset: 0x00062EA7
		public DynamicDrawManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600119A RID: 4506 RVA: 0x00064CC4 File Offset: 0x00062EC4
		public void RegisterDrawable(Thing t)
		{
			if (t.def.drawerType != DrawerType.None)
			{
				if (this.drawingNow)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(100, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Cannot register drawable ");
					defaultInterpolatedStringHandler.AppendFormatted<Thing>(t);
					defaultInterpolatedStringHandler.AppendLiteral(" while drawing is in progress. Things shouldn't be spawned in Draw methods.");
					Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.drawThings.Add(t);
			}
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x00064D28 File Offset: 0x00062F28
		public void DeRegisterDrawable(Thing t)
		{
			if (t.def.drawerType != DrawerType.None)
			{
				if (this.drawingNow)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(104, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Cannot deregister drawable ");
					defaultInterpolatedStringHandler.AppendFormatted<Thing>(t);
					defaultInterpolatedStringHandler.AppendLiteral(" while drawing is in progress. Things shouldn't be despawned in Draw methods.");
					Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.drawThings.Remove(t);
			}
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x00064D90 File Offset: 0x00062F90
		public void DrawDynamicThings()
		{
			if (!DebugViewSettings.drawThingsDynamic || this.map.Disposed)
			{
				return;
			}
			this.drawingNow = true;
			bool flag = SilhouetteUtility.CanHighlightAny();
			NativeArray<DynamicDrawManager.ThingCullDetails> details = new NativeArray<DynamicDrawManager.ThingCullDetails>(this.drawThings.Count, Allocator.TempJob, NativeArrayOptions.ClearMemory);
			this.ComputeCulledThings(details);
			if (!DebugViewSettings.singleThreadedDrawing)
			{
				using (new ProfilerBlock("Ensure Graphics Initialized"))
				{
					for (int i = 0; i < details.Length; i++)
					{
						if (details[i].shouldDraw)
						{
							this.drawThings[i].DynamicDrawPhase(DrawPhase.EnsureInitialized);
						}
					}
				}
				this.PreDrawVisibleThings(details);
			}
			try
			{
				using (new ProfilerBlock("Draw Visible"))
				{
					for (int j = 0; j < details.Length; j++)
					{
						if (details[j].shouldDraw || details[j].shouldDrawShadow)
						{
							try
							{
								if (details[j].shouldDraw)
								{
									this.drawThings[j].DynamicDrawPhase(DrawPhase.Draw);
								}
								else
								{
									Pawn pawn = this.drawThings[j] as Pawn;
									if (pawn != null)
									{
										pawn.DrawShadowAt(pawn.DrawPos);
									}
								}
							}
							catch (Exception value)
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 2);
								defaultInterpolatedStringHandler.AppendLiteral("Exception drawing ");
								defaultInterpolatedStringHandler.AppendFormatted<Thing>(this.drawThings[j]);
								defaultInterpolatedStringHandler.AppendLiteral(": ");
								defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
								Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
							}
						}
					}
				}
				if (flag)
				{
					this.DrawSilhouettes(details);
				}
			}
			catch (Exception value2)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler2.AppendLiteral("Exception drawing dynamic things: ");
				defaultInterpolatedStringHandler2.AppendFormatted<Exception>(value2);
				Log.Error(defaultInterpolatedStringHandler2.ToStringAndClear());
			}
			finally
			{
				details.Dispose();
			}
			this.drawingNow = false;
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x00064FF8 File Offset: 0x000631F8
		private void PreDrawVisibleThings(NativeArray<DynamicDrawManager.ThingCullDetails> details)
		{
			using (new ProfilerBlock("Pre draw job"))
			{
				new ManagedJobParallelFor(new DynamicDrawManager.PreDrawThings
				{
					details = details,
					things = this.drawThings
				}).Schedule(details.Length, UnityData.GetIdealBatchCount(details.Length), default(JobHandle)).Complete();
			}
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x0006507C File Offset: 0x0006327C
		private void ComputeCulledThings(NativeArray<DynamicDrawManager.ThingCullDetails> details)
		{
			CellRect cellRect = Find.CameraDriver.CurrentViewRect;
			cellRect = cellRect.ExpandedBy(1);
			if (WorldComponent_GravshipController.GravshipRenderInProgess)
			{
				cellRect = cellRect.Encapsulate(WorldComponent_GravshipController.GravshipRenderBounds);
			}
			cellRect.ClipInsideMap(this.map);
			using (new ProfilerBlock("Prepare cull job"))
			{
				for (int i = 0; i < this.drawThings.Count; i++)
				{
					Thing thing = this.drawThings[i];
					DynamicDrawManager.ThingCullDetails value = new DynamicDrawManager.ThingCullDetails
					{
						cell = ((thing is Pawn) ? thing.DrawPos.ToIntVec3() : thing.Position),
						coarseBounds = thing.OccupiedDrawRect(),
						hideAtSnowOrSandDepth = thing.def.hideAtSnowOrSandDepth,
						seeThroughFog = thing.def.seeThroughFog,
						hasSunShadows = thing.def.HasSunShadows,
						drawOffscreen = thing.def.drawOffscreen
					};
					details[i] = value;
				}
			}
			using (new ProfilerBlock("Cull job"))
			{
				new DynamicDrawManager.CullJob
				{
					indices = this.map.cellIndices,
					viewRect = cellRect,
					fogGrid = this.map.fogGrid.FogGrid_Unsafe,
					depthGrid = this.map.snowGrid.DepthGrid_Unsafe,
					details = details,
					checkShadows = MatBases.SunShadow.shader.isSupported,
					shadowViewRect = SectionLayer_SunShadows.GetSunShadowsViewRect(this.map, cellRect)
				}.Schedule(details.Length, UnityData.GetIdealBatchCount(details.Length), default(JobHandle)).Complete();
			}
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x00065278 File Offset: 0x00063478
		private void DrawSilhouettes(NativeArray<DynamicDrawManager.ThingCullDetails> details)
		{
			using (new ProfilerBlock("Prepare matrices job"))
			{
				for (int i = 0; i < details.Length; i++)
				{
					if (details[i].shouldDraw)
					{
						Thing thing = this.drawThings[i];
						if (SilhouetteUtility.ShouldDrawSilhouette(thing))
						{
							Pawn pawn = thing as Pawn;
							if (pawn != null)
							{
								DynamicDrawManager.ThingCullDetails value = details[i];
								value.pos = pawn.Drawer.renderer.SilhouettePos;
								value.drawSize = pawn.Drawer.renderer.SilhouetteGraphic.drawSize;
								value.drawSilhouette = true;
								details[i] = value;
							}
						}
					}
				}
			}
			using (new ProfilerBlock("Compute matrices"))
			{
				new DynamicDrawManager.ComputeSilhouetteMatricesJob
				{
					inverseFovScale = Find.CameraDriver.InverseFovScale,
					altitude = AltitudeLayer.Silhouettes.AltitudeFor(),
					details = details
				}.Schedule(details.Length, UnityData.GetIdealBatchCount(details.Length), default(JobHandle)).Complete();
			}
			using (new ProfilerBlock("Draw silhouettes"))
			{
				for (int j = 0; j < details.Length; j++)
				{
					if (details[j].drawSilhouette)
					{
						Pawn pawn2 = this.drawThings[j] as Pawn;
						if (pawn2 != null)
						{
							SilhouetteUtility.DrawSilhouetteJob(pawn2, details[j].trs);
						}
					}
				}
			}
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x00065448 File Offset: 0x00063648
		public void LogDynamicDrawThings()
		{
			Log.Message(DebugLogsUtility.ThingListToUniqueCountString(this.drawThings));
		}

		// Token: 0x04000FED RID: 4077
		private Map map;

		// Token: 0x04000FEE RID: 4078
		private readonly List<Thing> drawThings = new List<Thing>();

		// Token: 0x04000FEF RID: 4079
		private bool drawingNow;

		// Token: 0x020025C8 RID: 9672
		private struct ThingCullDetails
		{
			// Token: 0x04009AEB RID: 39659
			public IntVec3 cell;

			// Token: 0x04009AEC RID: 39660
			public CellRect coarseBounds;

			// Token: 0x04009AED RID: 39661
			public bool seeThroughFog;

			// Token: 0x04009AEE RID: 39662
			public float hideAtSnowOrSandDepth;

			// Token: 0x04009AEF RID: 39663
			public bool drawOffscreen;

			// Token: 0x04009AF0 RID: 39664
			public Vector3 pos;

			// Token: 0x04009AF1 RID: 39665
			public Vector2 drawSize;

			// Token: 0x04009AF2 RID: 39666
			public bool drawSilhouette;

			// Token: 0x04009AF3 RID: 39667
			public bool hasSunShadows;

			// Token: 0x04009AF4 RID: 39668
			public Matrix4x4 trs;

			// Token: 0x04009AF5 RID: 39669
			public bool shouldDraw;

			// Token: 0x04009AF6 RID: 39670
			public bool shouldDrawShadow;
		}

		// Token: 0x020025C9 RID: 9673
		[BurstCompile]
		private struct CullJob : IJobParallelFor
		{
			// Token: 0x0600F0DC RID: 61660 RVA: 0x00589014 File Offset: 0x00587214
			[BurstCompile]
			public void Execute(int index)
			{
				DynamicDrawManager.ThingCullDetails thingCullDetails = this.details[index];
				int num = this.indices[thingCullDetails.cell];
				if (!this.indices.Contains(num))
				{
					return;
				}
				if (!thingCullDetails.seeThroughFog && this.fogGrid.IsSet(num))
				{
					return;
				}
				if (thingCullDetails.hideAtSnowOrSandDepth < 1f && this.depthGrid[num] > thingCullDetails.hideAtSnowOrSandDepth)
				{
					return;
				}
				if (!thingCullDetails.drawOffscreen && !this.viewRect.Overlaps(thingCullDetails.coarseBounds))
				{
					if (this.checkShadows && thingCullDetails.hasSunShadows)
					{
						thingCullDetails.shouldDrawShadow = this.shadowViewRect.Contains(thingCullDetails.cell);
					}
					return;
				}
				thingCullDetails.shouldDraw = true;
				this.details[index] = thingCullDetails;
			}

			// Token: 0x04009AF7 RID: 39671
			[ReadOnly]
			public CellRect viewRect;

			// Token: 0x04009AF8 RID: 39672
			[ReadOnly]
			public CellRect shadowViewRect;

			// Token: 0x04009AF9 RID: 39673
			[ReadOnly]
			public CellIndices indices;

			// Token: 0x04009AFA RID: 39674
			[ReadOnly]
			public bool checkShadows;

			// Token: 0x04009AFB RID: 39675
			[ReadOnly]
			public NativeBitArray fogGrid;

			// Token: 0x04009AFC RID: 39676
			[ReadOnly]
			public NativeArray<float> depthGrid;

			// Token: 0x04009AFD RID: 39677
			public NativeArray<DynamicDrawManager.ThingCullDetails> details;
		}

		// Token: 0x020025CA RID: 9674
		[BurstCompile]
		private struct ComputeSilhouetteMatricesJob : IJobParallelFor
		{
			// Token: 0x0600F0DD RID: 61661 RVA: 0x005890E4 File Offset: 0x005872E4
			[BurstCompile]
			public void Execute(int index)
			{
				DynamicDrawManager.ThingCullDetails thingCullDetails = this.details[index];
				if (!thingCullDetails.drawSilhouette)
				{
					return;
				}
				Vector3 vector = new Vector3(thingCullDetails.drawSize.x, 0f, thingCullDetails.drawSize.y);
				Vector3 s = this.inverseFovScale;
				if (vector.x < 2.5f)
				{
					s.x *= vector.x + SilhouetteUtility.AdjustScale(vector.x);
				}
				else
				{
					s.x *= vector.x;
				}
				if (vector.z < 2.5f)
				{
					s.z *= vector.z + SilhouetteUtility.AdjustScale(vector.z);
				}
				else
				{
					s.z *= vector.z;
				}
				Vector3 pos = thingCullDetails.pos;
				pos.y = this.altitude;
				thingCullDetails.trs = Matrix4x4.TRS(pos, Quaternion.AngleAxis(0f, Vector3.up), s);
				this.details[index] = thingCullDetails;
			}

			// Token: 0x04009AFE RID: 39678
			public Vector3 inverseFovScale;

			// Token: 0x04009AFF RID: 39679
			public float altitude;

			// Token: 0x04009B00 RID: 39680
			public NativeArray<DynamicDrawManager.ThingCullDetails> details;
		}

		// Token: 0x020025CB RID: 9675
		private class PreDrawThings : IJobParallelFor
		{
			// Token: 0x0600F0DE RID: 61662 RVA: 0x005891E8 File Offset: 0x005873E8
			public void Execute(int index)
			{
				Thing thing = this.things[index];
				if (this.details[index].shouldDraw)
				{
					thing.DynamicDrawPhase(DrawPhase.ParallelPreDraw);
				}
			}

			// Token: 0x04009B01 RID: 39681
			public NativeArray<DynamicDrawManager.ThingCullDetails> details;

			// Token: 0x04009B02 RID: 39682
			public List<Thing> things;
		}
	}
}

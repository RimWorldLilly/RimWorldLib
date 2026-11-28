using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x020007FB RID: 2043
	public class Sustainer
	{
		// Token: 0x17000C79 RID: 3193
		// (get) Token: 0x0600407A RID: 16506 RVA: 0x0019AA6A File Offset: 0x00198C6A
		public bool Ended
		{
			get
			{
				return this.endRealTime >= 0f;
			}
		}

		// Token: 0x17000C7A RID: 3194
		// (get) Token: 0x0600407B RID: 16507 RVA: 0x0019AA7C File Offset: 0x00198C7C
		public float TimeSinceEnd
		{
			get
			{
				return Time.realtimeSinceStartup - this.endRealTime;
			}
		}

		// Token: 0x17000C7B RID: 3195
		// (get) Token: 0x0600407C RID: 16508 RVA: 0x0019AA8C File Offset: 0x00198C8C
		public float CameraDistanceSquared
		{
			get
			{
				if (this.info.IsOnCamera)
				{
					return 0f;
				}
				if (this.worldRootObject == null)
				{
					if (Prefs.DevMode)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Sustainer ");
						defaultInterpolatedStringHandler.AppendFormatted<SoundDef>(this.def);
						defaultInterpolatedStringHandler.AppendLiteral(" info is ");
						defaultInterpolatedStringHandler.AppendFormatted<SoundInfo>(this.info);
						defaultInterpolatedStringHandler.AppendLiteral(" but its worldRootObject is null");
						Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					return 0f;
				}
				return (float)(Find.CameraDriver.MapPosition - this.worldRootObject.transform.position.ToIntVec3()).LengthHorizontalSquared;
			}
		}

		// Token: 0x0600407D RID: 16509 RVA: 0x0019AB48 File Offset: 0x00198D48
		public Sustainer(SoundDef def, SoundInfo info)
		{
			this.def = def;
			this.info = info;
			if (def.subSounds.Count > 0)
			{
				foreach (KeyValuePair<string, float> keyValuePair in info.DefinedParameters)
				{
					this.externalParams[keyValuePair.Key] = keyValuePair.Value;
				}
				if (def.HasSubSoundsInWorld)
				{
					if (!info.forcedPlayOnCamera && info.IsOnCamera)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(61, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Playing sound ");
						defaultInterpolatedStringHandler.AppendFormatted<SoundDef>(def);
						defaultInterpolatedStringHandler.AppendLiteral(" on camera, but it has sub-sounds in the world.");
						Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					this.worldRootObject = new GameObject("SustainerRootObject_" + def.defName);
					this.UpdateRootObjectPosition();
				}
				else if (!info.IsOnCamera)
				{
					info = SoundInfo.OnCamera(info.Maintenance);
				}
				Find.SoundRoot.sustainerManager.RegisterSustainer(this);
				if (!info.IsOnCamera)
				{
					Find.SoundRoot.sustainerManager.UpdateAllSustainerScopes();
				}
				for (int i = 0; i < def.subSounds.Count; i++)
				{
					this.subSustainers.Add(new SubSustainer(this, def.subSounds[i]));
				}
			}
			if (Current.ProgramState != ProgramState.Playing)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.lastMaintainTick = Find.TickManager.TicksGame;
					this.lastMaintainFrame = Time.frameCount;
				});
				return;
			}
			this.lastMaintainTick = Find.TickManager.TicksGame;
			this.lastMaintainFrame = Time.frameCount;
		}

		// Token: 0x0600407E RID: 16510 RVA: 0x0019AD14 File Offset: 0x00198F14
		public void SustainerUpdate()
		{
			if (!this.Ended)
			{
				if (this.info.Maintenance == MaintenanceType.PerTick)
				{
					if (Find.TickManager.TicksGame > this.lastMaintainTick + 1)
					{
						this.End();
						return;
					}
				}
				else if (this.info.Maintenance == MaintenanceType.PerTickRare)
				{
					if (Find.TickManager.TicksGame > this.lastMaintainTick + 250)
					{
						this.End();
						return;
					}
				}
				else if (this.info.Maintenance == MaintenanceType.PerFrame && Time.frameCount > this.lastMaintainFrame + 1)
				{
					this.End();
					return;
				}
			}
			else if (this.TimeSinceEnd > this.def.sustainFadeoutTime)
			{
				this.Cleanup();
			}
			if (this.def.subSounds.Count > 0)
			{
				if (!this.info.IsOnCamera && this.info.Maker.HasThing)
				{
					this.UpdateRootObjectPosition();
				}
				this.scopeFader.SustainerScopeUpdate();
				for (int i = 0; i < this.subSustainers.Count; i++)
				{
					this.subSustainers[i].SubSustainerUpdate();
				}
			}
		}

		// Token: 0x0600407F RID: 16511 RVA: 0x0019AE2C File Offset: 0x0019902C
		private void UpdateRootObjectPosition()
		{
			Vector3 position = Vector3.zero;
			if (this.info.forcedPlayOnCamera)
			{
				position = Find.Camera.gameObject.transform.position;
			}
			else
			{
				position = this.info.Maker.Cell.ToVector3ShiftedWithAltitude(0f);
				Thing thing = this.info.Maker.Thing;
				if (thing != null)
				{
					position = thing.DrawPos.Yto0();
				}
			}
			if (this.worldRootObject != null)
			{
				this.worldRootObject.transform.position = position;
			}
		}

		// Token: 0x06004080 RID: 16512 RVA: 0x0019AEC8 File Offset: 0x001990C8
		public void Maintain()
		{
			if (this.Ended)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to maintain ended sustainer: ");
				defaultInterpolatedStringHandler.AppendFormatted<SoundDef>(this.def);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			if (this.info.Maintenance == MaintenanceType.PerTick || this.info.Maintenance == MaintenanceType.PerTickRare)
			{
				this.lastMaintainTick = Find.TickManager.TicksGame;
				return;
			}
			if (this.info.Maintenance == MaintenanceType.PerFrame)
			{
				this.lastMaintainFrame = Time.frameCount;
			}
		}

		// Token: 0x06004081 RID: 16513 RVA: 0x0019AF54 File Offset: 0x00199154
		public void End()
		{
			this.endRealTime = Time.realtimeSinceStartup;
			if (this.def.sustainFadeoutStartSound != null && this.def.sustainFadeoutTime >= 0.001f)
			{
				this.PlaySoundOnMapOrCamera(this.def.sustainFadeoutStartSound);
			}
			if (this.def.sustainFadeoutTime < 0.001f)
			{
				this.Cleanup();
			}
		}

		// Token: 0x06004082 RID: 16514 RVA: 0x0019AFB4 File Offset: 0x001991B4
		private void Cleanup()
		{
			if (this.def.subSounds.Count > 0)
			{
				Find.SoundRoot.sustainerManager.DeregisterSustainer(this);
				for (int i = 0; i < this.subSustainers.Count; i++)
				{
					this.subSustainers[i].Cleanup();
				}
			}
			if (this.def.sustainStopSound != null)
			{
				this.PlaySoundOnMapOrCamera(this.def.sustainStopSound);
			}
			if (this.worldRootObject != null)
			{
				Object.Destroy(this.worldRootObject);
			}
			DebugSoundEventsLog.Notify_SustainerEnded(this, this.info);
		}

		// Token: 0x06004083 RID: 16515 RVA: 0x0019B050 File Offset: 0x00199250
		private void PlaySoundOnMapOrCamera(SoundDef soundDef)
		{
			if (this.worldRootObject != null)
			{
				Map map = this.info.Maker.Map;
				if (map != null)
				{
					SoundInfo soundInfo = SoundInfo.InMap(new TargetInfo(this.worldRootObject.transform.position.ToIntVec3(), map, false), MaintenanceType.None);
					soundDef.PlayOneShot(soundInfo);
					return;
				}
			}
			else
			{
				soundDef.PlayOneShot(SoundInfo.OnCamera(MaintenanceType.None));
			}
		}

		// Token: 0x06004084 RID: 16516 RVA: 0x0019B0BC File Offset: 0x001992BC
		public string DebugString()
		{
			string text = this.def.defName;
			string str = text;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(18, 1);
			defaultInterpolatedStringHandler.AppendLiteral("\n  inScopePercent=");
			defaultInterpolatedStringHandler.AppendFormatted<float>(this.scopeFader.inScopePercent);
			text = str + defaultInterpolatedStringHandler.ToStringAndClear();
			string str2 = text;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(25, 1);
			defaultInterpolatedStringHandler2.AppendLiteral("\n  CameraDistanceSquared=");
			defaultInterpolatedStringHandler2.AppendFormatted<float>(this.CameraDistanceSquared);
			text = str2 + defaultInterpolatedStringHandler2.ToStringAndClear();
			foreach (SubSustainer value in this.subSustainers)
			{
				string str3 = text;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(8, 1);
				defaultInterpolatedStringHandler3.AppendLiteral("\n  sub: ");
				defaultInterpolatedStringHandler3.AppendFormatted<SubSustainer>(value);
				text = str3 + defaultInterpolatedStringHandler3.ToStringAndClear();
			}
			return text;
		}

		// Token: 0x04002AD5 RID: 10965
		public SoundDef def;

		// Token: 0x04002AD6 RID: 10966
		public SoundInfo info;

		// Token: 0x04002AD7 RID: 10967
		internal GameObject worldRootObject;

		// Token: 0x04002AD8 RID: 10968
		private int lastMaintainTick;

		// Token: 0x04002AD9 RID: 10969
		private int lastMaintainFrame;

		// Token: 0x04002ADA RID: 10970
		private float endRealTime = -1f;

		// Token: 0x04002ADB RID: 10971
		private List<SubSustainer> subSustainers = new List<SubSustainer>();

		// Token: 0x04002ADC RID: 10972
		public SoundParams externalParams = new SoundParams();

		// Token: 0x04002ADD RID: 10973
		public SustainerScopeFader scopeFader = new SustainerScopeFader();
	}
}

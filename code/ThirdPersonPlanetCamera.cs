using Sandbox;

namespace TLC.SSP
{
	public class ThirdPersonPlanetCamera : Camera
	{
		[ConVar.Replicated]
		public static bool thirdperson_orbit { get; set; } = false;

		[ConVar.Replicated]
		public static bool thirdperson_collision { get; set; } = true;

		private Angles orbitAngles;
		private float orbitDistance = 150;

		public Prop Planet;

		public override void Update()
		{
			if ( Planet == null ) return;

			var pawn = Local.Pawn as AnimEntity;
			var client = Local.Client;

			if ( pawn == null )
				return;

			Pos = pawn.Position;
			Vector3 targetPos;

			var planetUpVector = Pos - Planet.Position;
			planetUpVector = planetUpVector.Normal;
			var Rotation = Utils.FromToRotation( new Vector3( 0, 0, 1 ), planetUpVector );

			var center = pawn.Position + planetUpVector * 64;

			if ( thirdperson_orbit )
			{
				Pos += planetUpVector * (pawn.CollisionBounds.Center.z * pawn.Scale);
				Rot = Rotation.From( orbitAngles ) * Rotation;

				targetPos = Pos + Rot.Backward * orbitDistance;
			}
			else
			{
				var InputRotation = Input.Rotation * Rotation;
				Pos = center;
				Rot = Rotation.FromAxis( planetUpVector, 4 ) * InputRotation;

				float distance = 130.0f * pawn.Scale;
				targetPos = Pos + InputRotation.Right * ((pawn.CollisionBounds.Maxs.x + 15) * pawn.Scale);
				targetPos += InputRotation.Forward * -distance;
			}

			if ( thirdperson_collision )
			{
				var tr = Trace.Ray( Pos, targetPos )
					.Ignore( pawn )
					.Radius( 8 )
					.Run();

				Pos = tr.EndPos;
			}
			else
			{
				Pos = targetPos;
			}

			FieldOfView = 70;

			Viewer = null;
		}

		public override void BuildInput( InputBuilder input )
		{
			if ( Planet == null ) return;

			var planetUpVector = Pos - Planet.Position;
			planetUpVector = planetUpVector.Normal;
			var Rotation = Utils.FromToRotation( new Vector3( 0, 0, 1 ), planetUpVector );

			if ( thirdperson_orbit && input.Down( InputButton.Walk ) )
			{
				if ( input.Down( InputButton.Attack1 ) )
				{
					orbitDistance += input.AnalogLook.pitch;
					orbitDistance = orbitDistance.Clamp( 0, 1000 );
				}
				else
				{
					orbitAngles.yaw += input.AnalogLook.yaw;
					orbitAngles.pitch += input.AnalogLook.pitch;
					orbitAngles = orbitAngles.Normal;
					orbitAngles.pitch = orbitAngles.pitch.Clamp( -89, 89 );
				}

				input.AnalogLook = Angles.Zero;

				input.Clear();
				input.StopProcessing = true;
			}

			input.InputDirection *= Rotation;

			base.BuildInput( input );
		}
	}
}

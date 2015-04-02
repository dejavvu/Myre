﻿using Microsoft.Xna.Framework;
using Myre.Entities;
using Myre.Entities.Behaviours;
using Myre.Entities.Extensions;

namespace Myre.Graphics.Lighting
{
    public class PointLight
        : Behaviour
    {
        private static readonly TypedName<Vector3> _colourName = new TypedName<Vector3>("colour");
        private static readonly TypedName<Vector3> _positionName = new TypedName<Vector3>("position");
        private static readonly TypedName<float> _rangeName = new TypedName<float>("range");
        private static readonly TypedName<bool> _activeName = new TypedName<bool>("pointlight_active");

        private Property<Vector3> _colour;
        private Property<Vector3> _position;
        private Property<float> _range;
        private Property<bool> _active;

        public bool Active
        {
            get { return _active.Value; }
            set { _active.Value = value; }
        }

        public Vector3 Colour
        {
            get { return _colour.Value; }
            set { _colour.Value = value; }
        }

        public Vector3 Position
        {
            get { return _position.Value; }
            set { _position.Value = value; }
        }

        public float Range
        {
            get { return _range.Value; }
            set { _range.Value = value; }
        }

        public override void CreateProperties(Entity.ConstructionContext context)
        {
            _colour = context.CreateProperty(_colourName);
            _position = context.CreateProperty(_positionName);
            _range = context.CreateProperty(_rangeName);
            _active = context.CreateProperty(_activeName, true);
            
            base.CreateProperties(context);
        }

        public override void Initialise(Collections.INamedDataProvider initialisationData)
        {
            base.Initialise(initialisationData);

            initialisationData.TryCopyValue(this, _colourName, _colour);
            initialisationData.TryCopyValue(this, _positionName, _position);
            initialisationData.TryCopyValue(this, _rangeName, _range);
            initialisationData.TryCopyValue(this, _activeName, _active);
        }
    }
}

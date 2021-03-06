﻿using System.Numerics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.Linq;
using Myre.Extensions;

namespace Myre.Graphics.Geometry.Text
{
    public class VertexCharacter
    {
        private readonly Dictionary<char, float> _characterKerning;
        private readonly IReadOnlyDictionary<char, float> _readonlyCharacterKerning;

        /// <summary>
        /// Kerning to use to offset the next character in the string.
        /// i.e.
        /// Draw(ThisCharacter);
        /// MovePen(HorizontalCharacterKerning[nextCharacter] + ThisCharacter.Width)
        /// Draw(nextCharacter);
        /// </summary>
        public IReadOnlyDictionary<char, float> HorizontalCharacterKerning { get { return _readonlyCharacterKerning; } }

        private readonly Mesh _mesh;
        public Mesh Mesh { get { return _mesh; } }

        private readonly Vector2 _size;
        public Vector2 Size { get { return _size; } }

        private readonly char _character;
        public char Character { get { return _character; } }

        public VertexCharacter(char c, IEnumerable<KeyValuePair<char, float>> kerning, Mesh mesh, Vector2 width)
        {
            _character = c;

            _characterKerning = kerning.ToDictionary(a => a.Key, a => a.Value);
            _readonlyCharacterKerning = new Dictionary<char, float>(_characterKerning);

            _size = width;

            _mesh = mesh;
        }

        public float GetKern(char following)
        {
            float k;
            if (_characterKerning.TryGetValue(following, out k))
                return k;
            return 0;
        }
    }

    public class VertexCharacterReader : ContentTypeReader<VertexCharacter>
    {
        protected override VertexCharacter Read(ContentReader input, VertexCharacter existingInstance)
        {
            //Read character
            var character = input.ReadChar();

            //Read kerning table
            var kernCount = input.ReadInt32();
            KeyValuePair<char, float>[] kerning = new KeyValuePair<char, float>[kernCount];
            for (int i = 0; i < kernCount; i++)
                kerning[i] = new KeyValuePair<char, float>(input.ReadChar(), input.ReadSingle());

            //Read model data
            var model = input.ReadObject<Mesh>();

            //read width
            var size = input.ReadVector2().FromXNA();

            return new VertexCharacter(character, kerning, model, size);
        }
    }
}

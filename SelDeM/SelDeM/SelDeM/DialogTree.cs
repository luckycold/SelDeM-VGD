using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace SelDeM
{
    public class DialogTree<T>
    {
        private readonly T _value;
        private readonly List<DialogTree<T>> _children = new List<DialogTree<T>>();

        public DialogTree(T value)
        {
            _value = value;
        }

        public List<DialogTree<T>> Children
        {
            get { return _children; }
        }
        public DialogTree<T> this[int i]
        {
            get { return _children[i]; }
        }
        

        public DialogTree<T> Parent { get; private set; }

        public T Value { get { return _value; } }

        public DialogTree<T> AddChild(T value)
        {
            var node = new DialogTree<T>(value) { Parent = this };
            _children.Add(node);
            return node;
        }

        public DialogTree<T>[] AddChildren(params T[] values)
        {
            return values.Select(AddChild).ToArray();
        }

        public bool RemoveChild(DialogTree<T> node)
        {
            return _children.Remove(node);
        }

        public void Traverse(Action<T> action)
        {
            action(Value);
            foreach (var child in _children)
                child.Traverse(action);
        }
    }
}

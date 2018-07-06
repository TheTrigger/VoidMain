﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace VoidMain.CommandLineIinterface
{
    public class ColoredFormat : IReadOnlyList<Colored<object>>, IEnumerable<Colored<object>>
    {
        public Colored<string> Template { get; }
        public IFormatProvider FormatProvider { get; }
        private readonly List<Colored<object>> _args;

        public int Count => _args.Count;
        public Colored<object> this[int index] => _args[index];

        public ColoredFormat(
            Colored<string> template,
            IFormatProvider formatProvider = null)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }
            if (template.Value == null)
            {
                throw new ArgumentNullException(nameof(template) + "." + nameof(template.Value));
            }
            Template = template;
            FormatProvider = formatProvider;
            _args = new List<Colored<object>>();
        }

        public ColoredFormat(
            string template,
            Color foreground = null, Color background = null,
            IFormatProvider formatProvider = null)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }
            Template = new Colored<string>(template, foreground, background);
            FormatProvider = formatProvider;
            _args = new List<Colored<object>>();
        }

        public ColoredFormat Add(Colored<object> arg)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(nameof(arg));
            }
            _args.Add(arg);
            return this;
        }

        public ColoredFormat Add(object arg, Color foreground = null, Color background = null)
        {
            _args.Add(new Colored<object>(arg, foreground, background));
            return this;
        }

        public IEnumerator<Colored<object>> GetEnumerator() => _args.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _args.GetEnumerator();
    }
}
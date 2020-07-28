using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace VoidMain.IO.Keyboard
{
    public class KeyInputBuffer : IKeyReader, IDisposable
    {
        private readonly ChannelReader<KeyInput> _reader;
        private readonly ChannelWriter<KeyInput> _writer;
        private KeyInput _nextKey;
        private bool _hasNext;

        public bool FixNextKeyAvailabilityStatus { get; set; } = true;

        public KeyInputBuffer()
        {
            var options = new UnboundedChannelOptions
            {
                AllowSynchronousContinuations = false,
                SingleReader = true,
                SingleWriter = true
            };

            var channel = Channel.CreateUnbounded<KeyInput>(options);
            _reader = channel.Reader;
            _writer = channel.Writer;
        }

        public void Dispose() => _writer.TryComplete();

        public ValueTask<KeyInput> ReadKeyAsync(CancellationToken token = default)
        {
            return FixNextKeyAvailabilityStatus
                ? ReadKeyFixedAsync(token)
                : _reader.ReadAsync(token);
        }

        private async ValueTask<KeyInput> ReadKeyFixedAsync(CancellationToken token = default)
        {
            if (!_hasNext)
            {
                _nextKey = await _reader.ReadAsync(token).ConfigureAwait(false);
            }

            _hasNext = _reader.TryRead(out var nextKey);
            var key = new KeyInput(_nextKey.KeyInfo, _nextKey.Character, _hasNext);
            _nextKey = nextKey;
            return key;
        }

        public bool TryWriteKey(KeyInput input) => _writer.TryWrite(input);

        public void Clear()
        {
            if (_hasNext) _hasNext = false;
            while (_reader.TryRead(out _)) { }
        }
    }
}

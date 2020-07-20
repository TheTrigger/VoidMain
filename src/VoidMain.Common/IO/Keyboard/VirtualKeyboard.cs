using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace VoidMain.IO.Keyboard
{
    public class VirtualKeyboard : IKeyReader, IDisposable
    {
        private readonly Channel<KeyInput> _keysChannel;
        private readonly ChannelReader<KeyInput> _reader;
        private readonly ChannelWriter<KeyInput> _writer;
        private KeyInput _nextKey;
        private bool _hasNext;

        public bool FixNextKeyAvailabilityStatus { get; set; } = true;

        public VirtualKeyboard()
        {
            var options = new UnboundedChannelOptions
            {
                AllowSynchronousContinuations = false,
                SingleReader = true,
                SingleWriter = true
            };

            _keysChannel = Channel.CreateUnbounded<KeyInput>(options);
            _reader = _keysChannel.Reader;
            _writer = _keysChannel.Writer;
        }

        public void Dispose() => _writer.TryComplete();

        public ValueTask<KeyInput> ReadKeyAsync(CancellationToken token = default)
        {
            if (FixNextKeyAvailabilityStatus)
            {
                return ReadKeyFixedAsync(token);
            }
            else
            {
                return _reader.ReadAsync(token);
            }
        }

        private async ValueTask<KeyInput> ReadKeyFixedAsync(CancellationToken token = default)
        {
            if (!_hasNext)
            {
                _nextKey = await _reader.ReadAsync(token);
            }

            _hasNext = _reader.TryRead(out var nextKey);
            var key = new KeyInput(_nextKey.KeyInfo, _nextKey.Character, _hasNext);
            _nextKey = nextKey;
            return key;
        }

        public bool TryWriteKey(KeyInput input) => _writer.TryWrite(input);
    }
}

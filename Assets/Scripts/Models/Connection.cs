using System;

namespace Zachclone {
    public class Connection {
        public ConnectionType ConnectionType;
        public bool IsNonBlocking;

        public Connection(ConnectionType connectionType) {
            ConnectionType = connectionType;
        }
        
        private Wire _wire;

        public void Wire(Wire wire) {
            _wire = wire;
            _wire.AddConnection(this);
        }

        public void Unwire() {
            _wire = null;
        }

        public bool IsWired() {
            return _wire != null;
        }

        public bool HasValue() {
            return _wire.HasValue();
        }

        public void Write(int value) {
            if (!IsWired()) {
                throw new Exception("Write to unwired connection");
            }
            
            _wire.Write(value);
        }

        public int Read() {
            if (!IsWired()) {
                throw new Exception("Read to unwired connection");
            }
            return _wire.Read();
        }
    }
}
using System;

namespace Zachclone {
    public class Connection {
        public ConnectionType ConnectionType;
        public bool IsNonBlocking;
        public bool IsActiveWriter;

        public Connection(ConnectionType connectionType) {
            ConnectionType = connectionType;
        }
        
        public Wire Wire;

        public void SetWire(Wire wire) {
            Wire = wire;
            Wire.AddConnection(this);
        }

        public void RemoveWire() {
            Wire = null;
        }

        public bool IsWired() {
            return Wire != null;
        }

        public bool HasValue() {
            return Wire.HasValue();
        }

        public void Write(int value) {
            if (!IsWired()) {
                throw new Exception("Write to unwired connection");
            }
            
            Wire.Write(this, value);
        }

        public int Read() {
            if (!IsWired()) {
                throw new Exception("Read to unwired connection");
            }
            return Wire.Read();
        }
    }
}
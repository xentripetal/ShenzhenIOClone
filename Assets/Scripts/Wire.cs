using System.Collections.Generic;
using UnityEngine;
using Zachclone.Instructions.Models;

namespace Zachclone {
    public class Wire {
        public ConnectionType ConnectionType { get; protected set; }
        private bool isNonBlocking;
        private int value = -1000;
        private bool ConnectionTypeSet = false;
        private List<Connection> _connections = new List<Connection>();

        public bool HasValue() {
            if (!isNonBlocking)
                return value != -1000;
            return true;
        }
        
        public void AddConnection(Connection connection) {
            if (!ConnectionTypeSet) {
                ConnectionType = connection.ConnectionType;
                ConnectionTypeSet = true;
                if (connection.IsNonBlocking) {
                    isNonBlocking = true;
                }
            }

            if (connection.ConnectionType != ConnectionType) {
                throw new ChipLayoutException("Mismatch connection types");
            }

            if (ConnectionType == ConnectionType.SIMPLE) {
                value = 0;
            }
            else {
                value = -1000;
            }
            
            _connections.Add(connection);
        }

        public void RemoveConnection(Connection connection) {
            _connections.Remove(connection);
            if (_connections.Count == 0) {
                ConnectionTypeSet = false;
            }

            if (!connection.IsNonBlocking) return;
            
            isNonBlocking = false;
            foreach (var conn in _connections) {
                if (conn.IsNonBlocking) {
                    isNonBlocking = true;
                }
            }
        }

        public void Write(int value) {
            if (ConnectionType == ConnectionType.SIMPLE) {
                this.value = Mathf.Clamp(value, 0, 100);
            }
            else {
                if (this.value == -1000) {
                    this.value = value;
                } 
                else {
                    Debug.LogError("Write on write.");
                }
            }
        }

        public int Read() {
            if (ConnectionType == ConnectionType.SIMPLE) {
                return value;
            }

            if (isNonBlocking && value == -1000) {
                return -999;
            }

            var cachedValue = value;
            value = -1000;
            return cachedValue;
        }
    }
}
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zachclone.Instructions.Models;

namespace Zachclone {
    /// <summary>
    /// Blocking XBUS - Register reader, Register Writer (can happen in a single step)
    ///                 when both then send value to reader, release writer on next step. Remove value.
    /// Simple - Immediately write value, immediately read value. Value stays until written. 0 <= x <= 100
    /// Nonblocking XBUS - Writes stack up and advance, Reads pull from stack or give -999
    /// </summary>
    public class Wire {
        public ConnectionType ConnectionType { get; protected set; }
        public bool HasActiveWriter { get; protected set; }

        public bool HasActiveReader { get; protected set; }
        private int _rwCount;
        
        private bool isNonBlocking;
        private int value = -1000;
        private Stack<int> nonBlockingValues = new Stack<int>();
        private bool ConnectionTypeSet = false;
        private List<Connection> _connections = new List<Connection>();
        private Connection currentWriter;

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

        public void Write(Connection conn, int value) {
            if (ConnectionType == ConnectionType.SIMPLE) {
                this.value = Mathf.Clamp(value, 0, 100);
            }
            else if (isNonBlocking) {
                nonBlockingValues.Push(value);
            }
            else {
                if (HasActiveWriter && conn == currentWriter) {
                    if (HasActiveReader) {
                        _rwCount++;
                        if (_rwCount == 2) {
                            Reset();
                        }

                        return;
                    }
                }
                else if (!HasActiveWriter) {
                    currentWriter = conn;
                    conn.IsActiveWriter = true;
                    HasActiveWriter = true;
                    this.value = value;
                }
            }
        }

        private void Reset() {
            _rwCount = 0;
            currentWriter.IsActiveWriter = false;
            HasActiveWriter = false;
            HasActiveReader = false;
            currentWriter = null;
        }

        public int Read() {
            if (ConnectionType == ConnectionType.SIMPLE) {
                return value;
            }

            if (isNonBlocking) {
                if (nonBlockingValues.Count == 0) {
                    return -999;
                }

                return nonBlockingValues.Pop();
            }

            if (value == -1000) {
                HasActiveReader = true;
                return value;
            }

            _rwCount++;
            if (_rwCount == 2) {
                Reset();
            }
            
            var cachedValue = value;
            value = -1000;
            return cachedValue;
        }
    }
}
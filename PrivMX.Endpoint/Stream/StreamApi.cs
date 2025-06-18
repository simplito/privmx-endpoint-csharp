//
// PrivMX Endpoint C#
// Copyright © 2024 Simplito sp. z o.o.
//
// This file is part of the PrivMX Platform (https://privmx.dev).
// This software is Licensed under the MIT License.
//
// See the License for the specific language governing permissions and
// limitations under the License.
//

using PrivMX.Endpoint.Core;
using PrivMX.Endpoint.Core.Internal;
using PrivMX.Endpoint.Core.Models;
using PrivMX.Endpoint.Stream.Models;
using PrivMX.Endpoint.Store;
using PrivMX.Endpoint.Event;
using System;
using System.Collections.Generic;

namespace PrivMX.Endpoint.Stream
{
    public class StreamApi : IStreamApi
    {
        private readonly StoreApi storeApi;
        private readonly EventApi eventApi;

        static public StreamApi Create(Connection connection, StoreApi storeApi, EventApi eventApi)
        {
            return new StreamApi(connection, storeApi, eventApi);
        }

        private StreamApi(Connection connection, StoreApi storeApi, EventApi eventApi)
        {
            this.storeApi = storeApi;
            this.eventApi = eventApi;
        }

        public string CreateStreamRoom(string contextId, List<UserWithPubKey> users, List<UserWithPubKey> managers, byte[] publicMeta, byte[] privateMeta, ContainerPolicy policies = null)
        {
            return storeApi.CreateStore(contextId, users, managers, publicMeta, privateMeta, policy);
        }

        public void UpdateStreamRoom(string streamRoomId, List<UserWithPubKey> users, List<UserWithPubKey> managers, byte[] publicMeta, byte[] privateMeta, long version, bool force, bool forceGenerateNewKey, ContainerPolicy policies = null)
        {
            storeApi.UpdateStore(streamRoomId, users, managers, publicMeta, privateMeta, version, force, forceGenerateNewKey, policies);
        }

        public void DeleteStreamRoom(string streamRoomId)
        {
            storeApi.DeleteStore(streamRoomId);
        }

        public StreamRoom GetStreamRoom(string streamRoomId)
        {
            var store = storeApi.GetStore(streamRoomId);
            return MapStore(store);
        }

        public PagingList<StreamRoom> ListStreamRooms(string streamRoomId, PagingQuery pagingQuery)
        {
            var stores = storeApi.ListStores(streamRoomId, pagingQuery);
            return MapStores(stores);
        }

        public long CreateStream(string streamRoomId)
        {
            
        }

        void AddTrack(long streamId, TrackType type, string params = null)
        {

        }

        void PublishStream(long streamId)
        {

        }

        void StreamTrackSendData(long streamId, byte[] data)
        {

        }

        long JoinStream(string streamRoomId, string? settings = null)
        {

        }

        void StreamTrackRecvData(long streamId)
        {

        }

        void UnpublishStream(long streamId)
        {

        }

        void LeaveStream(long streamId)
        {

        }


        // private void SubscribeForStoreEvents(string contextId)
        // {

        // }

        // private void UnsubscribeFromStoreEvents(string contextId)
        // {
        // }

        // private void SubscribeForDataEvents(string streamRoomId)
        // {
        // }

        // private void UnsubscribeFromDataEvents(string streamRoomId)
        // {
        // }

        private PagingList<StreamRoom> MapStores(PagingList<Model.Store> stores)
        {
            PagingList<StreamRoom> result = new PagingList<StreamRoom>() { TotalAvailable = stores.TotalAvailable, ReadItems = [] };
            foreach (var store in stores.ReadItems)
            {
                result.ReadItems.Add(MapStore(store));
            }
            return result;
        }

        private StreamRoom MapStore(Models.Store store)
        {
            return new StreamRoom() {
                ContextId = store.ContextId,
                StreamRoomId = store.StoreId,
                CreateDate = store.CreateDate,
                Creator = store.Creator,
                LastModificationDate = store.LastModificationDate,
                LastModifier = store.LastModifier,
                Users = store.Users,
                Managers = store.Managers,
                Policy = store.Policy,
                PublicMeta = store.PublicMeta,
                PrivateMeta = store.PrivateMeta,
                StatusCode = store.StatusCode
            };
        }

    }
}

﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace StoreService {
    [ServiceContract]
    public interface IStoreService {
        /*[WebInvoke(Method="POST", UriTemplate="/tickets", BodyStyle=WebMessageBodyStyle.WrappedRequest, RequestFormat=WebMessageFormat.Json, ResponseFormat=WebMessageFormat.Json)]
        [OperationContract]
        int AddTicket(string author, string problem);

        [WebGet(UriTemplate="/tickets/{author}", ResponseFormat=WebMessageFormat.Json)]
        [OperationContract]
        DataTable GetTickets(string author);

        [WebGet(UriTemplate="/users", ResponseFormat=WebMessageFormat.Json)]
        [OperationContract]
        DataTable GetUsers();*/
        
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        void SellBook(int id, int bookId, int quantity, int clientId);

        [WebGet(UriTemplate = "/books", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        List<Book> GetBooks();

        [WebInvoke(Method = "POST", UriTemplate = "/orders", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Order CreateOrder(int clientId, int bookId, int quantity, bool instantSell);

        [WebInvoke(Method = "PATCH", UriTemplate = "/orders/{id}/state", BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        Order SetState(string id);
    }

    /**
     * Order for each book. Orders are always delivered at home, so there are no instant sells.
     * An Order may need to wait for the warehouse to delivered the respective book title.
     */
    [DataContract]
    public class Order {
        public class State {
            public enum Type {
                DELIVERED,
                WAITING,
                DISPATCH_OCCURS_AT,
                DISPATCHED_AT,
            }
            public Type type;
            public DateTime dispatchDate;
        }

        public Order(Client client, Book book, int quantity, bool instantSell) {
            guid = Guid.NewGuid();
            this.book = book;
            this.quantity = quantity;
            this.client = client;
            totalPrice = quantity * book.price;
            
            if (book.stock < quantity) {
                state = new State() { type = State.Type.WAITING };
            } else {
                if(instantSell) {
                    state = new State() { type = State.Type.DELIVERED, dispatchDate = DateTime.Now };
                } else {
                    state = new State() { type = State.Type.DISPATCH_OCCURS_AT, dispatchDate = DateTime.Now.AddDays(1) };
                }
            }
        }

        [DataMember]
        public Guid guid { get; private set; }

        [DataMember]
        public Book book { get; private set; }

        [DataMember]
        public int quantity { get; private set; }

        [DataMember]
        public Client client { get; private set; }

        [DataMember]
        public double totalPrice { get; private set; }

        [DataMember]
        public State state { get; private set; }
    }

    public class Client {
        public int id;
        public string email;
        public string name;
        public string address;
    }

    public class Book {
        public int id;
        public string title;
        public int stock;
        public double price;
    }
}
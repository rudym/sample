import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';


@Injectable()
export class CustomersApiService {
  url: string;
  entityId: string;

  constructor(@Inject('BASE_URL') private customersUrl, private http: HttpClient) {
    this.url = customersUrl;
  }


  //Customers endpoints
  getCustomers() {
    return this
      .http
      .get(`${this.url}customers`);
  };

  getCustomer(id: string) {
    return this
      .http
      .get(`${this.url}customers/${id}`);
  };

  updateCustomer(id: string, body: string) {
    return this
      .http
       .put(`${this.url}customers/${id}`, body);
  };

  createCustomer(body: string) {
    return this
      .http
      .post(`${this.url}customers`, body);
  };

  deleteCustomer(id: string) {
    return this
      .http
      .delete(`${this.url}customers/${id}`);
  };


  //Notes endpoints
  getNotes(entityId: string) {
    return this
      .http
      .get(`${this.url}notes/${entityId}`);
  };

  getNote(entityId: string, noteId: string) {
    return this
      .http
      .get(`${this.url}notes/${entityId}/${noteId}`);
  };

  updateNote(entityId: string, noteId: string, body: string) {
    return this
      .http
      .put(`${this.url}notes/${entityId}/${noteId}`, body);
  };

  createNote(body: string) {
    return this
      .http
      .post(`${this.url}notes`, body);
  };

  deleteNote(entityId: string, noteId: string,) {
    return this
      .http
      .delete(`${this.url}notes/${entityId}/${noteId}`);
  };
}

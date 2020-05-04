import { Injectable } from "@angular/core";
import {
  AuthSuccessResponse,
  IdentityClient,
  UserLoginRequest,
  UserRegistrationRequest,
} from "domain/client";
import { environment } from "src/environments/environment";

@Injectable({
  providedIn: "root",
})
export class AuthService {
  public user: AuthSuccessResponse;

  constructor() {}

  public sessionStorageAuthentication(): Promise<void> {
    return new Promise<void>((resolve, reject) => {
      const user = sessionStorage.getItem("user");
      try {
        this.user = Object.assign(new AuthSuccessResponse(), JSON.parse(user));
        if (!this.user.token) {
          this.user = null;
          throw new Error("no token found");
        }
        return resolve();
      } catch {
        sessionStorage.removeItem("user");
        return reject();
      }
    });
  }

  public register(email: string, password: string): Promise<void> {
    return new Promise<void>((resolve, reject) => {
      new IdentityClient(environment.baseUrl, null)
        .register(new UserRegistrationRequest({ email, password }))
        .then((result) => {
          if (!result) {
            reject("No response from server");
          }
          this.user = result;
          sessionStorage.setItem("user", JSON.stringify(this.user));
          resolve();
        })
        .catch((result) => {
          reject(result);
        });
    });
  }

  public login(email: string, password: string): Promise<void> {
    return new Promise<void>((resolve, reject) => {
      new IdentityClient(environment.baseUrl, null)
        .login(new UserLoginRequest({ email, password }))
        .then((result) => {
          if (!result) {
            reject("No response from server");
          }
          this.user = result;
          sessionStorage.setItem("user", JSON.stringify(this.user));
          resolve();
        })
        .catch((error: Error) => {
          if (error.message == "Failed to fetch")
            reject("We could access our servers at the moment =(");
          if (error.message == "An unexpected server error occurred.")
            reject("Invalid username and/or password.");
          reject(error.message);
        });
    });
  }
}

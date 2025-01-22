import {Order} from "../Types/Order";

let baseUrl = 'https://localhost:7043'

export const fetchOrders = async (): Promise<Order[]> => {
  const response = await fetch('https://localhost:7043/api/v1/order/get-all');
  //console.log(response.json());
  //return response.json();

  const data = await response.json();
  console.log(data);  // Adicione um log para garantir que os dados estão corretos

  return data.data as Order[];  
}

export const createOrder = async (quantity: number): Promise<any> => {
  const response = await fetch('https://localhost:7043/api/v1/order', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ quantity }),
  });
  return response.json();
};

export const completeOrder = async (orderId: string): Promise<boolean> => {
  const response = await fetch(`https://localhost:7043/api/v1/order/complete/${orderId}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
  });
  return response.ok;
};
